using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	Rigidbody rb;
	ProjectileObject projectile;

	float lifeSpan = 0f;
	bool effectCreated = false;

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

	public void Curve(float time, CurveShooter.Curve curve)
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
		CreateEffect();
        if (projectile.destroyOnContact)
			Destroy(gameObject);
	}

	void CreateEffect()
	{
        if (projectile.effect != null && !effectCreated)
        {
            GameObject obj = Instantiate(projectile.effect.gameObject, transform.position, Quaternion.identity);
            EffectArea e = obj.AddComponent<EffectArea>();
            e.Setup(projectile.effect);
			effectCreated = true;
        }
    }

	IEnumerator FollowCurve(float time, CurveShooter.Curve curve)
	{
		float elapsedTime = 0f;

		while (elapsedTime < time)
		{
			transform.position = curve.Evaluate(elapsedTime / time);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		CreateEffect();

		//Debug.Log("Boom");
		Destroy(gameObject);
	}
}
