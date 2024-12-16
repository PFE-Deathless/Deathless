using UnityEngine;

public class Sniper : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter shooter;
	public Transform cubeFeedback;
	public float maxScale = 0.7f;
	public float minScale = 0.1f;
	public AnimationCurve feedbackCurve;

	bool hasShot;

	protected override void PerformAttack()
	{
		switch (attackState)
		{
			case AttackState.Cast:
				Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
				float s = Mathf.Lerp(minScale, maxScale, feedbackCurve.Evaluate(stateTimer / cast));
				cubeFeedback.localScale = new Vector3(1f, s, 1f);
				break;
			case AttackState.Hit:
				if (!hasShot)
				{
					//Debug.Log("dzing !");
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
