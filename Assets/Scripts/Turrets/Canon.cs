using UnityEngine;

public class Canon : MonoBehaviour
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
	[SerializeField, Range(0f, 1f)] float shootAnimationPercentage = 0.5f;

	// Private properties
	Animator _animator; // 0.5s délai
	float _elapsedTime = 0f;
	float _elapsedTimeDelay = 0f;
	float _delay;
	bool _started = false;
	bool _shot;

	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_delay = 1f / frequency;
		_animator.SetFloat("AnimSpeed", frequency);

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
		if (_elapsedTime < _delay)
		{
			if (_elapsedTime / _delay > shootAnimationPercentage && !_shot)
			{
				Shoot();
				_shot = true;
			}
			_elapsedTime += Time.fixedDeltaTime;
		}
		else
		{
			_animator.SetTrigger("Shoot");
			_shot = false;
			_elapsedTime = 0f;
		}
	}

	public void Shoot()
	{
		//_animator.SetTrigger("Shoot");
		GameObject obj = Instantiate(projectile, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
		obj.GetComponent<Projectile>().Setup(projectileLifeSpan, destroyOnImpact);
		obj.GetComponent<Rigidbody>().linearVelocity = obj.transform.forward * projectileSpeed;
	}
}
