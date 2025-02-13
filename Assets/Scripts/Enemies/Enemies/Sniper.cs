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
		Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
	}

	protected override void StartHit()
	{
		shooter.ShootProjectile();
	}
}
