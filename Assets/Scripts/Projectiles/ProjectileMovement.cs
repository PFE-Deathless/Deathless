using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	Rigidbody rb;
	ProjectileObject projectile;

	float lifeSpan = 0f;

	public void Setup(ProjectileObject projectile)
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		rb.linearVelocity = transform.forward * projectile.speed;
		this.projectile = projectile;
	}

	private void Update()
	{
		lifeSpan += Time.deltaTime;
		if (lifeSpan >= projectile.lifeSpan)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerHealth p = other.gameObject.GetComponent<PlayerHealth>();
		if (p != null)
			p.TakeDamage(projectile.damage);
		Destroy(gameObject);
	}
}
