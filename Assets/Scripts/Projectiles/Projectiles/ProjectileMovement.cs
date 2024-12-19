using System.Collections;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	Rigidbody rb;
	ProjectileObject projectile;

	float lifeSpan = 0f;
	bool effectCreated = false;
	ProjectilePrevisualisation previsualisation;

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
		projectile.type = ProjectileObject.Type.Forward;
		rb.linearVelocity = transform.forward * projectile.speed;
	}

	public void Homing()
	{
		projectile.type = ProjectileObject.Type.Homing;
		// Nope
		Debug.Log("nope");
	}

	public void Curve(float time, CurveShooter.Curve curve)
	{
		projectile.type = ProjectileObject.Type.Curve;
		GameObject obj = Instantiate(projectile.previsualisation, curve.Evaluate(0.99f), Quaternion.identity);
		previsualisation = obj.GetComponent<ProjectilePrevisualisation>();
		previsualisation.Setup(projectile.effect.radius);
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
		if (projectile.type != ProjectileObject.Type.Curve)
		{
			PlayerHealth p = other.gameObject.GetComponent<PlayerHealth>();
			if (p != null)
				p.TakeDamage(projectile.damage);
			CreateEffect();
			if (projectile.destroyOnContact)
				Destroy(gameObject);      
		}
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
			previsualisation.SetSize(elapsedTime / time);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		previsualisation.Destroy();
		CreateEffect();

		Debug.Log("Boom");
		Destroy(gameObject);
	}
}
