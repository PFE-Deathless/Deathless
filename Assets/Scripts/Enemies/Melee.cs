using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : Enemy
{
	[Header("Statistics")]
	public float radius = 4f;
	public GameObject hitCollider;

	PlayerHealth playerHealth;

	protected override void EnemyStart()
	{
		hitCollider.transform.localScale = new Vector3(radius * 2f, 0.5f, radius * 2f);
	}

	protected override void PerformAttack()
	{
		switch (attackState)
		{
			case AttackState.Cast:
				break;
			case AttackState.Hit:

				if (playerHealth == null)
				{
					Collider[] c = Physics.OverlapSphere(transform.position, radius, playerLayerMask);
					if (c.Length > 0)
						playerHealth = c[0].GetComponent<PlayerHealth>();
					if (playerHealth != null)
						playerHealth.TakeDamage(1);
				}

				hitCollider.SetActive(true);
				break;
			case AttackState.Cooldown:
				playerHealth = null;
				hitCollider.SetActive(false);
				break;
			case AttackState.None:
				break;
		}
	}
}
