using UnityEngine;

public class Sniper : Enemy
{
	public ProjectileShooter shooter;

	bool hasShot;

	protected override void PerformAttack()
	{
		switch (attackState)
		{
			case AttackState.Cast:
				Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
				break;
			case AttackState.Hit:
				if (!hasShot)
				{
					Debug.Log("dzing !");
					shooter.ShootProjectile();
					hasShot = true;
				}
				break;
			case AttackState.Cooldown:
				break;
			case AttackState.None:
				hasShot = false;
				break;
		}
	}
}
