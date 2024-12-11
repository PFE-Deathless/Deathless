using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	[Header("Statistics")]
	public int healthMax = 3;
	public float range = 10f;
	public float moveSpeed = 8f;

	[Header("Patrol")]
	public float waitingTime = 2f;
	public float stoppingDistance = 2f;
	public Vector3[] patrolPoints;

	[Header("Technical")]
	public LayerMask playerLayerMask;
	public TextMeshPro debugText;

	public HitType.Type currentType { get; private set; }
	public HitType.Type[] types { get; private set; }

	int health;
	HitBar hitBar;
	NavMeshAgent navMeshAgent;
	Transform target;

	void Start()
	{
		hitBar = GetComponentInChildren<HitBar>();
		SetupNavMeshAgent();
		SetTypes();
		currentType = types[0];
		health = healthMax;
		StartCoroutine(Patrol());
	}

	private void Update()
	{
		
	}

	bool DetectPlayer()
	{
		Collider[] p = new Collider[1];
		if (Physics.OverlapSphereNonAlloc(transform.position, range, p, playerLayerMask) > 0)
		{
			//Debug.Log("Player detected ! ");
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

		currentType = types[healthMax - health];
		hitBar.UpdateHitBar(healthMax - health);
	}

	void SetTypes()
	{
		types = new HitType.Type[healthMax];
		for (int i = 0; i < healthMax; i++)
		{
			types[i] = HitType.GetRandomType();
		}
		hitBar.SetTypes(types);
	}

	// ### COROUTINES ###

	IEnumerator Search()
	{
		debugText.text = "SEARCH";

		if (DetectPlayer())
			StartCoroutine(GoToPlayer());
		else
			StartCoroutine(Wait());
		yield break;
	}

	IEnumerator Patrol()
	{
		debugText.text = "PATROL";

		if (patrolPoints.Length == 0)
		{
			StartCoroutine(Search());
			yield break;
		}
		Vector3 dest = patrolPoints[Random.Range(0, patrolPoints.Length)];

		navMeshAgent.SetDestination(dest);
		while (Vector3.Distance(transform.position, dest) > 2.0f)
		{
			if (DetectPlayer())
			{
				StartCoroutine(GoToPlayer());
				yield break;
			}
			yield return null;
		}

		StartCoroutine(Search());
		yield break;
	}
	IEnumerator GoToPlayer()
	{
		debugText.text = "GO_TO_PLAYER";

		while (Vector3.Distance(transform.position, target.position) > 2.0f)
		{
			navMeshAgent.SetDestination(target.position);
			yield return null;
		}

		StartCoroutine(Attack());
		yield break;
	}

	IEnumerator Wait()
	{
		debugText.text = "WAIT";

		yield return new WaitForSeconds(waitingTime);
		StartCoroutine(Patrol());
		yield break;
	}

	protected virtual IEnumerator Attack()
	{
		debugText.text = "ATTACK";

		Debug.Log("Paf !");
		StartCoroutine(Wait());
		yield break;
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
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