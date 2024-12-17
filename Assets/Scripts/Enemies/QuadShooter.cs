using UnityEngine;

public class QuadShooter : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter[] shooters;
	public Transform feedbackCylinder;
	public Transform start;
	public Transform end;

	bool hasShot;

	protected override void PerformAttack()
	{
		switch (attackState)
		{
			case AttackState.Cast:
				//Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 120f);
				feedbackCylinder.position = Vector3.Lerp(start.position, end.position, stateTimer / cast);
				break;
			case AttackState.Hit:
				if (!hasShot)
				{
					Debug.Log("dzing !");
					foreach (ProjectileShooter shooter in shooters)
					{
						shooter.ShootProjectile();
					}
					hasShot = true;
				}
				break;
			case AttackState.Cooldown:
				feedbackCylinder.position = Vector3.Lerp(end.position, start.position, (stateTimer - (cast + hit)) / cooldown);
				break;
			case AttackState.None:
				hasShot = false;
				break;
		}
	}
}
