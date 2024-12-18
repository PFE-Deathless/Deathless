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

	// Attack
	protected AttackState attackState;


	// State Machine
	EnemyState state;
	float stateTimer;
	Vector3 patrolDestination;

	// DEBUG
	LineRenderer destLR;

	void Start()
	{
		hitBar = GetComponentInChildren<HitBar>();
		SetupNavMeshAgent();
		SetTypes();
		CurrentType = Types[0];
		health = healthMax;

		ChangeState(EnemyState.Patrol);

		destLR = gameObject.AddComponent<LineRenderer>();
		destLR.startWidth = 0.2f;
		destLR.endWidth = 0.2f;
	}

	private void Update()
	{
		HandleStates();

		debugText.enabled = showState;

		destLR.SetPosition(0, transform.position);
		if (target != null)
			destLR.SetPosition(1, target.position);
		else
			destLR.SetPosition(1, transform.position);
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
			GetComponent<LootManager>().instantiateLoot(transform.position);
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
		Cooldown,
		None
	}

	void HandleStates()
	{
		AnyState();

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

	void AnyState()
	{
		if (target != null && Vector3.Distance(transform.position, target.position) > maxRange)
		{
			target = null;
			ChangeState(EnemyState.Patrol);
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

		if (distance <= acquisitionRange)
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
			ChangeState(EnemyState.Patrol);
		}
	}

	void Attack()
	{
		debugText.text = "ATTACK";

		navMeshAgent.isStopped = true;

		float cast = attackCastTime;
		float hit = cast + attackDuration;
		float cooldown = hit + attackCooldown;
		
		stateTimer += Time.deltaTime;

		switch (stateTimer)
		{
			case var _ when stateTimer < cast:
				debugText.text = "ATTACK_CAST";
				attackState = AttackState.Cast;
				break;
			case var _ when stateTimer >= cast && stateTimer < hit:
				debugText.text = "ATTACK_HIT";
				attackState = AttackState.Hit;
				break;
			case var _ when stateTimer >= hit && stateTimer < cooldown:
				debugText.text = "ATTACK_CD";
				attackState = AttackState.Cooldown;
				break;
			case var _ when stateTimer >= cooldown:
				attackState = AttackState.None;
				break;
		}
	
		PerformAttack();

		if (attackState == AttackState.None)
			ChangeState(EnemyState.Patrol);
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
		navMeshAgent.isStopped = false;
		if (newState == EnemyState.Patrol)
			ChooseNewPatrolPoint();
		state = newState;
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

