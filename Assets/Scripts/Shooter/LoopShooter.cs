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
	}

	protected void Update()
	{
		if (started)
			PerformShoot();
	}

	protected void PerformShoot()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1f / shootFrequency)
		{
			shooter.ShootProjectile();
			elapsedTime -= 1f / shootFrequency;
		}
	}

	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(offsetDelay);
		started = true;
	}
}
