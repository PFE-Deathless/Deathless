using UnityEngine;

public class Shocker : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] float frequency = 1f;
	[SerializeField] float startDelay;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] float projectileLifeSpan = 10f;
	[SerializeField] bool destroyOnImpact = true;

	[Header("Technical")]
	[SerializeField] GameObject projectile;
	[SerializeField] Transform origin;

	float _elapsedTime = 0f;
	float _elapsedTimeDelay = 0f;
	float _delay;
	bool _started = false;
	private void Start()
	{
		_delay = 1f / frequency;

		if (origin == null)
			origin = transform;
	}

	private void FixedUpdate()
	{
		if (_started)
			PerformShoot();
		else
		{
			if (_elapsedTimeDelay < startDelay)
				_elapsedTimeDelay += Time.fixedDeltaTime;
			else
				_started = true;
		}
	}

	protected void PerformShoot()
	{
		_elapsedTime += Time.fixedDeltaTime;

		if (_elapsedTime >= _delay)
		{
			Shoot();
			_elapsedTime = 0f;
		}
	}

	public void Shoot()
	{
		GameObject obj = Instantiate(projectile, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
		obj.GetComponent<Projectile>().Setup(projectileLifeSpan, destroyOnImpact);
		obj.GetComponent<Rigidbody>().linearVelocity = obj.transform.forward * projectileSpeed;
	}
}
