//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class Charger : Enemy
//{
//	[Header("Statistics")]
//	public float maxChargeDistance = 10f;
//	public float chargeSpeed = 20f;
//	public LayerMask goThroughLayer = (1 << 7 | 1 << 20);
//	public GameObject test;
	
//	Vector3 targetPosition;
//	float originalSpeed;
//	float originalAcceleration;

//	protected override void EnemyStart()
//	{
//		originalSpeed = navMeshAgent.speed;
//		originalAcceleration = navMeshAgent.acceleration;
//	}

//	protected override void PerformAttack()
//	{
//		test.transform.position = targetPosition;
//		switch (attackState)
//		{
//			case AttackState.Cast:
//				Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
//				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f);
				
//				targetPosition = transform.position + transform.forward * maxChargeDistance;

//				if (Physics.Raycast(transform.position + (Vector3.up / 2f), transform.forward, out RaycastHit h, maxChargeDistance, goThroughLayer))
//				{
//					targetPosition = transform.position + transform.forward * (h.distance - 0.5f);
//				}

//				navMeshAgent.speed = chargeSpeed;
//				navMeshAgent.acceleration = 50000f;
//				break;
//			case AttackState.Hit:
//				navMeshAgent.isStopped = false;
//				navMeshAgent.SetDestination(targetPosition);
//				break;
//			case AttackState.Cooldown:
//				navMeshAgent.speed = originalSpeed;
//				navMeshAgent.acceleration = originalAcceleration;
//				break;
//			case AttackState.None:
//				break;
//		}
//	}
//}
