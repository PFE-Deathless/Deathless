using UnityEngine;

public class Sniper : Enemy
{
	[Header("Projectile")]
	[SerializeField] Transform projectileOrigin;
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] float projectileSpeed;
	[SerializeField] float projectileLifespan;
	[SerializeField] bool projectileDestroyOnImpact = true;

	protected override void StartCast()
	{
		if (animator != null)
			animator.SetTrigger("Attack");
	}

	protected override void UpdateCast()
	{
		Vector3 direction = target.position - transform.position;
		direction.y = 0f;
		direction.Normalize();

		Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
	}

	protected override void StartHit()
	{
		GameObject obj = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation, GameManager.Instance.ProjectileParent);
		obj.GetComponent<Projectile>().Setup(projectileLifespan, projectileDestroyOnImpact);
		obj.GetComponent<Rigidbody>().linearVelocity = obj.transform.forward * projectileSpeed;
	}
}
