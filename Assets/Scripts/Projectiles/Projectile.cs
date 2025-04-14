using UnityEngine;

public class Projectile : MonoBehaviour
{
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
			Destroy(gameObject, 0.02f);
	}
}
