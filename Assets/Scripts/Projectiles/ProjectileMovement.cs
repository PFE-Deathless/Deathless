using System.Collections;
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
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		this.projectile = projectile;
	}

	public void Forward()
	{
		rb.linearVelocity = transform.forward * projectile.speed;
	}

	public void Homing()
	{
		// Nope
		Debug.Log("nope");
	}

	public void Curve(float time, QuadraticCurve.Curve curve)
	{
		StartCoroutine(FollowCurve(time, curve));
	}

	private void Update()
	{
		LifeSpan();
	}

	void LifeSpan()
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
		if (projectile.destroyOnContact)
			Destroy(gameObject);
	}

	IEnumerator FollowCurve(float time, QuadraticCurve.Curve curve)
	{
		float elapsedTime = 0f;

		while (elapsedTime < time)
		{
			transform.position = curve.Evaluate(elapsedTime / time);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Debug.Log("Boom");
		Destroy(gameObject);
	}
}
