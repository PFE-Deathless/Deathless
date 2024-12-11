using UnityEngine;

public class Sniper : Enemy
{
	public ProjectileShooter shooter;

	bool hasShot;

	protected override void PerformAttack()
	{
		if (!hasShot)
		{
			Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1200f);
			shooter.ShootProjectile();
			hasShot = true;
		}
		Debug.Log("dzing !");
	}

	protected override void ChangeAttackState(AttackState newState)
	{
		base.ChangeAttackState(newState);
		if (newState == AttackState.Cooldown)
		{
			hasShot = false;
		}
	}
}
