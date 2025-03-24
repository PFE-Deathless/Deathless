using UnityEngine;

public class Sniper : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter shooter;

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
		shooter.ShootProjectile();
	}
}
