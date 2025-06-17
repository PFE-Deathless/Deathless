using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] GameObject impactParticlePrefab;

	bool _destroyOnImpact = true;

	public void Setup(float lifeSpan, bool destroyOnImpact)
	{
		_destroyOnImpact = destroyOnImpact;
		StartCoroutine(DestroyProjectile(lifeSpan));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
			PlayerHealth.Instance.TakeDamage(1);
		if (_destroyOnImpact)
		{
			if (impactParticlePrefab != null)
				Instantiate(impactParticlePrefab, transform.position, Quaternion.identity, GameManager.Instance.ProjectileParent);
			StartCoroutine(DestroyProjectile());
		}
	}

	IEnumerator DestroyProjectile(float delay = 0f)
	{
		yield return new WaitForSeconds(delay);

		GetComponentInChildren<Rigidbody>().linearVelocity = Vector3.zero;

		foreach (Collider collider in GetComponentsInChildren<Collider>())
			collider.enabled = false;

		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
			renderer.enabled = false;

		foreach (TrailRenderer trail in GetComponentsInChildren<TrailRenderer>())
			trail.enabled = false;

		foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
			ps.Stop();

		Destroy(gameObject, 3f);
	}
}
