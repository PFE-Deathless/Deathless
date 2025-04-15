using UnityEngine;

public class CurveShooter : MonoBehaviour
{
	[Header("Shooter")]
	[Tooltip("Minimum range of detection")] public float minRange = 1f;
	[Tooltip("Maximum range of detection")] public float maxRange = 10f;
	[Tooltip("Frequency at which projectile will be fired")] public float shootFrequency = 1f;
	[Tooltip("Time it takes for a projectile to reach its destination")] public float time = 1f;
	[Tooltip("Height of the parabola that the projectile will follow")] public float height = 2f;

	[Header("Technical")]
	[Tooltip("Origin of the projectile that will be fired")] public Transform origin;
	[Tooltip("Canon that will be rotated")] public Transform canon;
	[Tooltip("Projectile that will be fired")] public GameObject projectile;
	[Tooltip("Layer of the player for detection")] public LayerMask playerLayer = (1 << 3 | 1 << 6);

	[Header("Debug")]
	[SerializeField] bool showRange = false;

	float elapsedTime = 0f;
	Transform target;

	public class Curve
	{
		Vector3 a;
		Vector3 b;
		Vector3 c;

		public Curve(Vector3 start, Vector3 end, float height)
		{
			a = start;
			b = end;
			c = Vector3.Lerp(a, b, 0.5f);
			c.y += height;
		}

		public Vector3 Evaluate(float t)
		{
			Vector3 ac = Vector3.Lerp(a, c, t);
			Vector3 cb = Vector3.Lerp(c, b, t);
			return Vector3.Lerp(ac, cb, t);
		}

		public Quaternion EvaluateRotation(float t)
		{
			return Quaternion.LookRotation((Evaluate(Mathf.Clamp01(t + Time.deltaTime)) - Evaluate(Mathf.Clamp01(t))).normalized);
		}
	}

	void Update()
	{
		target = GetTarget();
		if (target != null && canon != null)
		{
			canon.rotation = new Curve(origin.position, target.position, height).EvaluateRotation(0f);
		}
	}

	private void FixedUpdate()
	{
		PerformShoot();
	}

	public void ShootProjectile()
	{
		if (origin == null)
			origin = transform;
		GameObject obj = Instantiate(projectile, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
		obj.GetComponent<CurveProjectile>().Setup(new Curve(origin.position, target.position, height), time);
	}

	protected Transform GetTarget()
	{
		Collider[] cMax = Physics.OverlapSphere(transform.position, maxRange, playerLayer);
		if (cMax.Length > 0)
		{
			Collider[] cMin = Physics.OverlapSphere(transform.position, minRange, playerLayer);
			if (cMin.Length > 0)
				return null;
			else
				return cMax[0].transform;
		}
		return null;
	}

	protected void PerformShoot()
	{
		if (elapsedTime > 0f)
		{
			elapsedTime -= Time.fixedDeltaTime;
		}
		else
		{
			if (target != null)
			{
				ShootProjectile();
				elapsedTime = 1f / shootFrequency;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (showRange)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, minRange);
			Gizmos.DrawWireSphere(transform.position, maxRange);
		}
	}
}
