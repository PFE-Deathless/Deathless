using UnityEngine;

public class Shocker : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter shooter;
	public Transform spawnStart;
	public Transform spawnEnd;
	public Transform spawnTransform;
	public TrailRenderer spawnTrail;

	bool hasShot;

	protected override void PerformAttack()
	{
		switch (attackState)
		{
			case AttackState.Cast:
				Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
				spawnTrail.emitting = true;
				spawnTransform.position = Vector3.Lerp(spawnStart.position, spawnEnd.position, stateTimer / attackCastTime);
				break;
			case AttackState.Hit:
				spawnTrail.emitting = false;
				if (!hasShot)
				{
					Debug.Log("shockwave !");
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
