using UnityEngine;

public class QuadShooter : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter[] shooters;
	public Transform feedbackCylinder;
	public Transform start;
	public Transform end;

	protected override void UpdateCast()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 120f);
		feedbackCylinder.position = Vector3.Lerp(start.position, end.position, stateTimer / cast);
	}

	protected override void StartHit()
	{
		foreach (ProjectileShooter shooter in shooters)
		{
			shooter.ShootProjectile();
		}
	}

	protected override void UpdateCooldown()
	{
		feedbackCylinder.position = Vector3.Lerp(end.position, start.position, (stateTimer - (cast + hit)) / cooldown);
	}
}
