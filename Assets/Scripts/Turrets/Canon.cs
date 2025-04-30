using UnityEngine;

public class Canon : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField, Tooltip("Delay before the turret starts to shoot")] float startDelay;
	[SerializeField, Tooltip("Speed of the projectile shot")] float projectileSpeed = 10f;
	[SerializeField, Tooltip("Life SPan of the projectile, it will be destroyed after this time")] float projectileLifeSpan = 10f;
	[SerializeField, Tooltip("If the projectile should be destroyed if it touches something")] bool destroyOnImpact = true;

	[Header("Bursts")]
	[SerializeField, Tooltip("Number of projectiles in a burst")] int projectilesPerBurst = 1;
	[SerializeField, Tooltip("Delay between bursts")] float delayBetweenBursts = 0f;
	[SerializeField, Tooltip("Delay between projectile in a burst")] float delayBetweenProjectiles = 0.5f;

	[Header("SFX")]
	[SerializeField] AudioEntry audioShoot;

	[Header("Technical")]
	[SerializeField] GameObject projectile;
	[SerializeField] Transform origin;
	[SerializeField, Range(0f, 1f)] float shootAnimationPercentage = 0.5f;

	// Private properties
	Animator _animator; // 0.5s délai
	float _elapsedTimeDelay = 0f;
	bool _started = false;
	bool _shot;
	int _projectilesInBurst;
	float _burstDelayElapsedTime;
	float _projectileDelayElapsedTime;

	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		if (_animator != null)
			_animator.SetFloat("AnimSpeed", 1f / delayBetweenProjectiles);

		_projectileDelayElapsedTime = delayBetweenProjectiles;

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
		if (_projectilesInBurst < projectilesPerBurst)
		{
			if (_projectileDelayElapsedTime < delayBetweenProjectiles)
			{
				if (_projectileDelayElapsedTime / delayBetweenProjectiles > shootAnimationPercentage && !_shot)
				{
					if (_animator != null)
						_animator.SetTrigger("Shoot");
					_shot = true;
				}
				_projectileDelayElapsedTime += Time.fixedDeltaTime;
			}
			else
			{
				Shoot();
				_shot = false;
				_projectilesInBurst++;
				_projectileDelayElapsedTime = 0f;
			}
		}
		else
		{
			if (_burstDelayElapsedTime < delayBetweenBursts)
			{
				_burstDelayElapsedTime += Time.fixedDeltaTime;
			}
			else
			{
				_projectilesInBurst = 0;
				_burstDelayElapsedTime = 0f;
			}
		}
	}

	public void Shoot()
	{
		GameObject obj = Instantiate(projectile, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
		AudioManager.Instance.Play(audioShoot, transform.position);
		obj.GetComponent<Projectile>().Setup(projectileLifeSpan, destroyOnImpact);
		obj.GetComponent<Rigidbody>().linearVelocity = obj.transform.forward * projectileSpeed;
	}
}
