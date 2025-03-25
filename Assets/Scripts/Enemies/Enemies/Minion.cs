using UnityEngine;

public class Minion : Enemy
{
	[SerializeField] GameObject hitCollider;

	// Called at the Start() of the enemy
	protected override void EnemyStart()
	{

	}

	// Called once when the enemy is casting its attack
	protected override void StartCast()
	{
		if (animator != null)
			animator.SetTrigger("Attack");
	}

	// Called every frame when the enemy is casting its attack
	protected override void UpdateCast()
	{
		Vector3 direction = target.position - transform.position;
		direction.y = 0f;
		direction.Normalize();

		Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
	}

	// Called once when the enemy is using its attack
	protected override void StartHit()
	{
		hitCollider.SetActive(true);
	}

	// Called every frame when the enemy is using its attack
	protected override void UpdateHit()
	{

	}

	// Called once when the enemy's attack is in cooldown
	protected override void StartCooldown()
	{
		hitCollider.SetActive(false);
	}

	// Called every frame when the enemy's attack is in cooldown
	protected override void UpdateCooldown()
	{

	}

	// Called once when the enemy has finished its attack
	protected override void EndAttack()
	{

	}
}
