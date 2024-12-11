using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	[Header("Statistics")]
	public int healthMax = 3;
	public float range = 5f;
	public float acquisitionRange = 2f;
	public float maxRange = 10f;
	public float moveSpeed = 8f;

	[Header("Attack")]
	public float attackCastTime = 0.1f;
	public float attackDuration = 0.5f;
	public float attackCooldown = 1f;

	[Header("Patrol")]
	public float waitingTime = 2f;
	public float stoppingDistance = 2f;
	public Vector3[] patrolPoints;

	[Header("Technical")]
	public bool showState;
	public LayerMask playerLayerMask;
	public TextMeshPro debugText;

	public HitType.Type CurrentType { get; private set; }
	public HitType.Type[] Types { get; private set; }

	int health;
	HitBar hitBar;
	NavMeshAgent navMeshAgent;
	protected Transform target;


	// State Machine
	EnemyState state;
	float stateTimer;
	Vector3 patrolDestination;
	AttackState attackState;

	void Start()
	{
		hitBar = GetComponentInChildren<HitBar>();
		SetupNavMeshAgent();
		SetTypes();
		CurrentType = Types[0];
		health = healthMax;

		ChangeState(EnemyState.Patrol);
		ChooseNewPatrolPoint();
	}

	private void Update()
	{
		HandleStates();

		debugText.enabled = showState;
	}

	bool DetectPlayer()
	{
		Collider[] p = new Collider[1];
		if (Physics.OverlapSphereNonAlloc(transform.position, range, p, playerLayerMask) > 0)
		{
			target = p[0].transform;

			return true;
		}
		return false;
	}

	void SetupNavMeshAgent()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = moveSpeed;
		navMeshAgent.stoppingDistance = stoppingDistance;
	}

	public void TakeDamage()
	{
		health--;
		if (health <= 0)
		{
			Destroy(gameObject);
			return;
		}

		CurrentType = Types[healthMax - health];
		hitBar.UpdateHitBar(healthMax - health);
	}

	protected virtual void PerformAttack()
	{
		Debug.Log("Paf!");
	}

	void SetTypes()
	{
		Types = new HitType.Type[healthMax];
		for (int i = 0; i < healthMax; i++)
		{
			Types[i] = HitType.GetRandomType();
		}
		hitBar.SetTypes(Types);
	}

	// #####################
	// #####################
	// ### STATE MACHINE ###
	// #####################
	// #####################
	
	#region state_machine

	public enum EnemyState
	{
		Patrol,
		GoToPlayer,
		Attack,
		Wait
	}

	public enum AttackState
	{
		Cast,
		Hit,
		Cooldown
	}

	void HandleStates()
	{
		switch (state)
		{
			case EnemyState.Patrol:
				Patrol();
				break;
			case EnemyState.GoToPlayer:
				GoToPlayer();
				break;
			case EnemyState.Attack:
				Attack();
				break;
			case EnemyState.Wait:
				Wait();
				break;
		}
	}

	void Patrol()
	{
		debugText.text = "PATROL";

		if (patrolPoints.Length == 0)
		{
			Debug.LogError("NO PATROL POINTS !!");
			ChangeState(EnemyState.Wait);
			return;
		}

		if (Vector3.Distance(transform.position, patrolDestination) <= stoppingDistance)
		{
			ChangeState(EnemyState.Wait);
		}
		else
		{
			navMeshAgent.SetDestination(patrolDestination);
			if (DetectPlayer())
			{
				ChangeState(EnemyState.GoToPlayer);
			}
		}
	}

	void GoToPlayer()
	{
		debugText.text = "GO_TO_PLAYER";

		float distance = Vector3.Distance(transform.position, target.position);

		if (distance > maxRange)
		{
			ChooseNewPatrolPoint();
			ChangeState(EnemyState.Patrol);
		}
		else if (distance <= acquisitionRange)
		{
			ChangeState(EnemyState.Attack);
		}
		else
		{
			navMeshAgent.SetDestination(target.position);
		}
	}

	void Wait()
	{
		debugText.text = "WAIT";

		if (DetectPlayer())
			ChangeState(EnemyState.GoToPlayer);

		stateTimer += Time.deltaTime;
		if (stateTimer >= waitingTime)
		{
			ChooseNewPatrolPoint();
			ChangeState(EnemyState.Patrol);
		}
	}

	void Attack()
	{
		debugText.text = "ATTACK";

		navMeshAgent.isStopped = true;

		stateTimer += Time.deltaTime;

		if (stateTimer < attackCastTime)
		{
			debugText.text = "ATTACK_CAST";
			ChangeAttackState(AttackState.Cast);
		}

		if (stateTimer >= attackCastTime && stateTimer < attackDuration + attackCastTime)
		{
			debugText.text = "ATTACK_HIT";
			ChangeAttackState(AttackState.Hit);
			PerformAttack();
		}
		else if (stateTimer >= attackCooldown)
		{
			ChangeState(EnemyState.Patrol);
		}

		if (stateTimer < attackCooldown && stateTimer >= attackDuration + attackCastTime)
		{
			debugText.text = "ATTACK_CD";
			ChangeAttackState(AttackState.Cooldown);
		}
	}

	void ChooseNewPatrolPoint()
	{
		if (patrolPoints.Length > 0)
		{
			patrolDestination = patrolPoints[Random.Range(0, patrolPoints.Length)];
		}
	}

	void ChangeState(EnemyState newState)
	{
		stateTimer = 0f;
		state = newState;
	}

	protected virtual void ChangeAttackState(AttackState newState)
	{
		attackState = newState;
	}

	#endregion

	// ### COROUTINES ###


#if UNITY_EDITOR
	[CustomEditor(typeof(Enemy), true), CanEditMultipleObjects]
	public class EnemyEditor : Editor
	{
		Enemy _Enemy;

		public void OnEnable()
		{
			_Enemy = (Enemy)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}

		public void OnSceneGUI()
		{
			Handles.color = Color.magenta;
			if (_Enemy.patrolPoints.Length > 0)
			{
				for (int i = 0; i < _Enemy.patrolPoints.Length; i++)
				{
					EditorGUI.BeginChangeCheck();
					_Enemy.patrolPoints[i] = Handles.PositionHandle(_Enemy.patrolPoints[i], Quaternion.identity);
					if (EditorGUI.EndChangeCheck()) // Check if position has changed
					{
						EditorUtility.SetDirty(_Enemy); // Mark the object as dirty to save the changes
					}
					Handles.DrawLine(_Enemy.transform.position, _Enemy.patrolPoints[i], 10.0f);
				}
			}
		}
	}
#endif
}

