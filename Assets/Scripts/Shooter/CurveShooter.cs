using UnityEngine;

[RequireComponent(typeof(QuadraticCurve))]
[RequireComponent(typeof(ProjectileShooter))]
public class CurveShooter : MonoBehaviour
{
	public float minRange = 1f;
	public float maxRange = 10f;
	public float shootFrequency = 1f;
	public LayerMask playerLayer = (1 << 3 | 1 << 6);

	ProjectileShooter projectileShooter;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		projectileShooter = GetComponent<ProjectileShooter>();
	}

	// Update is called once per frame
	void Update()
	{
		//Collider[] c = Physics.OverlapSphere(transform.position, maxRange, playerLayer);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, minRange);
		Gizmos.DrawWireSphere(transform.position, maxRange);
	}
}
