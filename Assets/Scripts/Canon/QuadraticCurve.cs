using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
	public Transform origin;
	public Transform target;
	public float height;

	Vector3 control;

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
	}

	private void Start()
	{
		if (origin == null)
			origin = transform;
	}

	void Update()
	{
		control = Vector3.Lerp(origin.position, target.position, 0.5f);
		control.y += height;
	}

	public Curve GetCurve(Vector3 target)
	{
		return new Curve(origin.position, target, height);
	}

	private void OnDrawGizmos()
	{
		if (target == null || origin == null)
			return;
		control = Vector3.Lerp(origin.position, target.position, 0.5f);
		control.y += height;

		Gizmos.DrawLine(origin.position, target.position);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(control, 0.3f);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(target.position, 0.3f);
		
		Gizmos.color = Color.green;
		for (int i = 0; i < 20; i++)
		{
			Gizmos.DrawWireSphere(GetCurve(target.position).Evaluate(i / 20f), 0.1f);
		}
	}
}
