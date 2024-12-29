using UnityEngine;

public class Sniper : Enemy
{
	[Header("Projectile")]
	public ProjectileShooter shooter;
	public Transform cubeFeedback;
	public float maxScale = 0.7f;
	public float minScale = 0.1f;
	public AnimationCurve feedbackCurve;

	protected override void UpdateCast()
	{
		Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
		float s = Mathf.Lerp(minScale, maxScale, feedbackCurve.Evaluate(stateTimer / cast));
		cubeFeedback.localScale = new Vector3(1f, s, 1f);
	}

	protected override void StartHit()
	{
		shooter.ShootProjectile();
	}
}
