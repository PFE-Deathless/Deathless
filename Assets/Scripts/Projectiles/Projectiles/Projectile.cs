using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float lifeSpan = 10f;

	void Start()
	{
		Destroy(gameObject, lifeSpan);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			PlayerHealth.Instance.TakeDamage(1);
		}
		Destroy(gameObject, 0.02f);
	}
}
