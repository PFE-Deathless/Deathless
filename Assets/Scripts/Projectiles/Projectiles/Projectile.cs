using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] float lifeSpan = 10f;
	[SerializeField] bool destroyOnImpact = true;

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
		if (destroyOnImpact)
			Destroy(gameObject, 0.02f);
	}
}
