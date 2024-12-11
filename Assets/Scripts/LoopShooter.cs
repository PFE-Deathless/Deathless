using UnityEngine;

public abstract class LoopShooter : MonoBehaviour
{
	public float shootFrequency = 5f;

	protected float elapsedTime = 0f;
	protected ProjectileShooter shooter;

	void Start()
	{
		shooter = GetComponent<ProjectileShooter>();
	}

	protected void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1f / shootFrequency)
		{
			shooter.ShootProjectile();
			elapsedTime -= 1f / shootFrequency;
		}
	}
}
