using System.Collections;
using UnityEngine;

[RequireComponent (typeof(ProjectileShooter))]
public class LoopShooter : MonoBehaviour
{
	public float shootFrequency = 5f;
	public float offsetDelay = 0f;

	protected float elapsedTime = 0f;
	protected ProjectileShooter shooter;

	protected bool started = false;

	void Start()
	{
		shooter = GetComponent<ProjectileShooter>();
		StartCoroutine(DelayStart());
	}

	protected void FixedUpdate()
	{
		if (started)
			PerformShoot();
	}

	protected void PerformShoot()
	{
		elapsedTime += Time.fixedDeltaTime;
		if (elapsedTime >= 1f / shootFrequency)
		{
			shooter.ShootProjectile();
			elapsedTime = 0f;
		}
	}

	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(offsetDelay);
		started = true;
	}
}
