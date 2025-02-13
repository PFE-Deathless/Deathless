using UnityEngine;

public class Shocker : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter shooter;
	public Transform spawnStart;
	public Transform spawnEnd;
	public Transform spawnTransform;
	public TrailRenderer spawnTrail;

	protected override void StartCast()
	{
		spawnTrail.emitting = true;
	}

	protected override void UpdateCast()
	{
		Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
		spawnTransform.position = Vector3.Lerp(spawnStart.position, spawnEnd.position, stateTimer / attackCastTime);
	}

	protected override void StartHit()
	{
		spawnTrail.emitting = false;
		shooter.ShootProjectile();
	}
}
