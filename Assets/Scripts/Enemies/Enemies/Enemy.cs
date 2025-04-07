using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] HitType.Type[] weaknesses = new HitType.Type[1];
	//[Tooltip("Minimum souls the enemy gives when dying")] public int minSouls = 3;
	//[Tooltip("Maximum souls the enemy gives when dying")] public int maxSouls = 8;
	[Tooltip("Player detection range, range at which the enemy can detect the player")] public float range = 5f;
	[Tooltip("Range at which the enemy will consider being close enough to perform its attack")] public float acquisitionRange = 2f;
	[Tooltip("Maximum range before the enemy drops the aggro")] public float maxRange = 10f;
	[SerializeField] float chargeMoveSpeed = 8f;
	[SerializeField] float patrolMoveSpeed = 4f;

	[Header("Attack")]
	public float attackWaitTime = 1f;
	public float attackCastTime = 0.1f;
	public float attackDuration = 0.5f;
	public float attackCooldown = 1f;

	[Header("Damage")]
	[SerializeField] protected float blinkingTime = 0.25f;
	[SerializeField] protected float shakeDuration = 0.3f;
	[SerializeField] protected float deathDuration = 1f;
	[SerializeField] protected float deathDisplacement = 2f;

	[Header("Patrol")]
	public float waitingTime = 2f;
	public float stoppingDistance = 0.1f;
	public Vector3[] patrolPoints;

	[Header("VFX")]
	public GameObject slashObject;
	public Transform slashTransform;
	public Transform aggroTransform;
	public GameObject damageParticle;
	public GameObject aggroFeedbackPrefab;
	public GameObject preHitFeedbackPrefab;

	[Header("Technical")]
	public bool showState;
	public bool showPathToTarget;
	public LayerMask playerLayerMask = (1 << 3) | (1 << 6);
	public TextMeshPro debugText;
	public Transform mesh;

	public HitType.Type CurrentType { get; private set; }
	public HitType.Type[] Weaknesses => weaknesses;

	int healthMax;
	int health;
	HitBar hitBar;
	protected NavMeshAgent navMeshAgent;
	protected Transform target;
	protected Material defaultMaterial;

	// Attack
	protected AttackState attackState = AttackState.None;
	protected float attackElapsedTime = 0f;
	protected float stateTimer;
	protected float wait;
	protected float cast;
	protected float hit;
	protected float cooldown;

	// Damage taken
	BlinkingMaterials blinkingMaterials;
	ParticleSystem damagePS;
	protected Material blinkingMaterial;
	protected bool isBlinking = false;
	protected float currentBlinkingTime;
	protected bool gotDamaged = false;
	protected float _shakeElapsedTime = 0f;

	// Death

	// State Machine
	EnemyState state;
	Vector3 patrolDestination;

	// Animation
	protected Animator animator;

	// DEBUG
	LineRenderer destLR;

	class BlinkingMaterials
	{
		public List<SkinnedMeshRenderer> skinnedMeshRenderers;
		public List<Material[]> defaultMaterials;
		public List<Material[]> blinkingMaterials;
		public Material blinkingMaterial;
		public Color originalColor;

		public BlinkingMaterials(Material blinkingMaterial)
		{
			skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
			defaultMaterials = new List<Material[]>();
			blinkingMaterials = new List<Material[]>();
			this.blinkingMaterial = blinkingMaterial;
		}

		public void Add(SkinnedMeshRenderer skinnedMeshRenderer)
		{
			skinnedMeshRenderers.Add(skinnedMeshRenderer);
			defaultMaterials.Add(skinnedMeshRenderer.materials);
			Material[] bs = new Material[skinnedMeshRenderer.materials.Length];
			for (int i = 0; i < bs.Length; i++) bs[i] = blinkingMaterial;
			blinkingMaterials.Add(bs);
		}

		public void Blink(bool state)
		{
			for (int i = 0; i < skinnedMeshRenderers.Count; i++)
			{
				skinnedMeshRenderers[i].materials = state ? blinkingMaterials[i] : defaultMaterials[i];
			}
		}
	}

	void Start()
	{
		hitBar = GetComponentInChildren<HitBar>();
		animator = GetComponentInChildren<Animator>();
		if (damageParticle != null)
			damagePS = damageParticle.GetComponent<ParticleSystem>();
		SetupNavMeshAgent();
		//SetTypes();
		hitBar.SetTypes(weaknesses, 0);
		CurrentType = weaknesses[0];
		healthMax = weaknesses.Length;
		health = healthMax;

		debugText.gameObject.SetActive(showState);

		GetMeshRenderersAndMaterials();


		EnemyStart();


		ChangeState(EnemyState.Patrol);

		destLR = gameObject.AddComponent<LineRenderer>();
		destLR.startWidth = 0.2f;
		destLR.endWidth = 0.2f;

		wait = attackWaitTime;
		cast = wait + attackCastTime;
		hit = cast + attackDuration;
		cooldown = hit + attackCooldown;
	}

	private void Update()
	{
		HandleStates();

		HandleBlink();

		HandleShake();

		if (animator != null)
			animator.SetFloat("Speed", navMeshAgent.velocity.sqrMagnitude / navMeshAgent.speed);

		debugText.enabled = showState;

		destLR.SetPosition(0, transform.position);
		if (target != null && showPathToTarget)
			destLR.SetPosition(1, target.position);
		if (target == null || !showPathToTarget)
			destLR.SetPosition(1, transform.position);
	}

	protected virtual void EnemyStart() { }

	void GetMeshRenderersAndMaterials()
	{
		Material blinkingMaterial = Resources.Load<Material>("Materials/M_BlinkDamageEnemies");
		blinkingMaterials = new(blinkingMaterial);

		SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer smr in smrs)
		{
			blinkingMaterials.Add(smr);
		}
	}

	void HandleBlink()
	{
		if (isBlinking)
		{
			if (Time.time <= currentBlinkingTime + blinkingTime)
			{
				blinkingMaterials.Blink(true);
			}
			else
				isBlinking = false;
		}
		else
		{
			blinkingMaterials.Blink(false);
		}
	}

	void HandleShake()
	{
		if (_shakeElapsedTime >= 0)
		{
			float strength = (_shakeElapsedTime / shakeDuration) * 0.5f;
			float shakeX = (Mathf.PerlinNoise(Time.time * 20f, 0) - 0.5f) * 2f * strength;
			float shakeZ = (Mathf.PerlinNoise(0, Time.time * 20f) - 0.5f) * 2f * strength;
			_shakeElapsedTime -= Time.deltaTime;

			mesh.localPosition = new(shakeX, 0f, shakeZ);
		}
		else
		{
			mesh.localPosition = Vector3.zero;
		}
	}

	bool DetectPlayer()
	{
		if (target != null)
			return true;
		Collider[] p = new Collider[1];
		if (Physics.OverlapSphereNonAlloc(transform.position, range, p, playerLayerMask) > 0)
		{
			target = p[0].transform;

			if (aggroFeedbackPrefab != null)
				Instantiate(aggroFeedbackPrefab, aggroTransform.position, Quaternion.identity, transform);

			return true;
		}
		return false;
	}

	void SetupNavMeshAgent()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = patrolMoveSpeed;
		navMeshAgent.stoppingDistance = stoppingDistance;
	}

	public void TakeDamage()
	{
		if (state == EnemyState.Death)
			return;

		health--;
		if (slashObject != null && slashTransform != null)
		{
			GameObject obj = Instantiate(slashObject, slashTransform.position, Quaternion.LookRotation(transform.position - PlayerController.Instance.transform.position));
			Destroy(obj, 5f);
		}
		if (damagePS != null)
			damagePS.Play();
		gotDamaged = true;
		_shakeElapsedTime = shakeDuration;
		if (health <= 0)
		{
			Kill();
			return;
		}

		// Blink
		currentBlinkingTime = Time.time;
		isBlinking = true;

		CurrentType = Weaknesses[healthMax - health];
		hitBar.SetTypes(Weaknesses, healthMax - health);
		hitBar.Shake();
	}

	public void Kill()
	{
		GameObject obj = Instantiate(Resources.Load<GameObject>("Barks/Bark"), transform.position, Quaternion.identity);
		obj.GetComponent<BarkObject>().InitBark();
		//PlayerSouls.Instance.AddSouls(Random.Range(minSouls, maxSouls + 1));
		
		ChangeState(EnemyState.Death);
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
		Wait,
		Death
	}

	public enum AttackState
	{
		Wait,
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
				navMeshAgent.speed = patrolMoveSpeed;
				Patrol();
				break;
			case EnemyState.GoToPlayer:
				navMeshAgent.speed = chargeMoveSpeed;
				GoToPlayer();
				break;
			case EnemyState.Attack:
				Attack();
				break;
			case EnemyState.Wait:
				Wait();
				break;
			case EnemyState.Death:
				Death();
				break;
		}
	}

	void AnyState()
	{
		if (state == EnemyState.Death)
			return;

		if (gotDamaged)
		{
			gotDamaged = false;
			animator.SetTrigger("CancelAttack");
			ChangeState(EnemyState.GoToPlayer);
		}

		if (target != null)
		{
			if (Vector3.Distance(transform.position, target.position) > maxRange)
			{
				target = null;
				ChangeState(EnemyState.Patrol);
			}
		}
	}

	void Death()
	{
		hitBar.gameObject.SetActive(false);
		GetComponent<Collider>().enabled = false;
		animator.SetTrigger("CancelAttack");
		animator.SetFloat("Speed", 0f);
		navMeshAgent.isStopped = true;

		if (stateTimer < deathDuration)
		{
			Vector3 newPos = transform.position;
			newPos.y = (stateTimer / deathDuration) * -deathDisplacement;
			transform.position = newPos;
			stateTimer += Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
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
				ChangeState(EnemyState.GoToPlayer);
		}
	}

	void GoToPlayer()
	{
		debugText.text = "GO_TO_PLAYER";

		float distance = Vector3.Distance(transform.position, target.position);

		if (distance <= acquisitionRange && !NavMesh.Raycast(transform.position, target.position, out NavMeshHit hit, NavMesh.AllAreas))
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
		navMeshAgent.isStopped = true;

		switch (stateTimer)
		{
			case var _ when stateTimer < wait:
				if (attackState != AttackState.Wait)
				{
					stateTimer = 0f;
					debugText.text = "ATTACK_WAIT";
					attackState = AttackState.Wait;
				}
				if (attackElapsedTime < wait)
				{
					attackElapsedTime += Time.deltaTime;
				}
				break;
			case var _ when stateTimer >= wait && stateTimer < cast:
				if (attackState != AttackState.Cast)
				{
					stateTimer = wait;

					if (preHitFeedbackPrefab != null)
						Instantiate(preHitFeedbackPrefab, aggroTransform.position, Quaternion.identity, transform);

					debugText.text = "ATTACK_CAST";
					StartCast();
					attackState = AttackState.Cast;
				}
				UpdateCast();
				break;
			case var _ when stateTimer >= cast && stateTimer < hit:
				if (attackState != AttackState.Hit)
				{
					stateTimer = cast;
					debugText.text = "ATTACK_HIT";
					StartHit();
					attackState = AttackState.Hit;
				}
				UpdateHit();
				break;
			case var _ when stateTimer >= hit && stateTimer < cooldown:
				if (attackState != AttackState.Cooldown)
				{
					stateTimer = hit;
					debugText.text = "ATTACK_CD";
					StartCooldown();
					attackState = AttackState.Cooldown;
				}
				UpdateCooldown();
				break;
			case var _ when stateTimer >= cooldown:
				EndAttack();
				attackState = AttackState.None;
				break;
		}

		stateTimer += Time.deltaTime;

		if (attackState == AttackState.None)
			ChangeState(EnemyState.Patrol);
	}


	// Methods called for each phase of the attack (Start once and Update every frame)
	protected virtual void StartCast() { }
	protected virtual void UpdateCast() { }
	protected virtual void StartHit() { }
	protected virtual void UpdateHit() { }
	protected virtual void StartCooldown() { }
	protected virtual void UpdateCooldown() { }
	protected virtual void EndAttack() { }

	void ChooseNewPatrolPoint()
	{
		if (patrolPoints.Length > 0)
		{
			List<Vector3> tempPatrols = patrolPoints.ToList();
			tempPatrols.Remove(patrolDestination);
			patrolDestination = tempPatrols[Random.Range(0, tempPatrols.Count)];
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

	private void OnValidate()
	{
		debugText.enabled = showState;
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(Enemy), true), CanEditMultipleObjects]
	public class EnemyEditor : Editor
	{
		Enemy _Enemy;

		public void OnEnable()
		{
			_Enemy = (Enemy)target;
			if (_Enemy.patrolPoints.Length == 0)
				_Enemy.patrolPoints = new Vector3[1];
			if (_Enemy.patrolPoints.Length == 1)
				_Enemy.patrolPoints[0] = _Enemy.transform.position + Vector3.forward * 2f;
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

