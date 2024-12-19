using System.Collections;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
	[SerializeField, Tooltip("Default to this transform if null")] protected Transform target;
	[SerializeField, Tooltip("Delay before it starts its cycle")] float offsetDelay = 0f;

	bool started = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Setup();
	}

	void FixedUpdate()
	{
		if (started)
			PerformMove();
	}

	protected virtual void Setup()
	{
		StartCoroutine(DelayStart());
		if (target == null)
			target = transform;
	}

	protected virtual void PerformMove()
	{

	}

	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(offsetDelay);
		started = true;
	}
}
