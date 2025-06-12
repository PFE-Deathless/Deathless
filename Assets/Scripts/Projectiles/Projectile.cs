using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] GameObject impactParticlePrefab;

	bool _destroyOnImpact = true;

	public void Setup(float lifeSpan, bool destroyOnImpact)
	{
		_destroyOnImpact = destroyOnImpact;
		Destroy(gameObject, lifeSpan);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
			PlayerHealth.Instance.TakeDamage(1);
		if (_destroyOnImpact)
		{
			if (impactParticlePrefab != null)
				Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
			Destroy(gameObject, 0.02f);
		}
	}
}
