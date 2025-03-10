using System.Collections;
using UnityEngine;

public class Canon : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] float frequency = 1f;
	[SerializeField] float startDelay;
	[SerializeField] float projectileSpeed = 10f;


	[Header("Technical")]
	[SerializeField] GameObject projectile;
	[SerializeField] Transform origin;

	// Private properties
	Animator _animator; // 0.5s délai
	float _elapsedTime = 0f;
	float _animationDelay;
	float _delay;
	bool _animStarted = false;
	bool _started = false;

	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_delay = 1f / frequency;
		_animator.SetFloat("AnimSpeed", frequency);
		_animationDelay = _delay / 2f;

		if (origin == null)
			origin = transform;

		StartCoroutine(DelayStart());
	}

	private void FixedUpdate()
	{
		if (_started)
			PerformShoot();
	}

	protected void PerformShoot()
	{
		_elapsedTime += Time.fixedDeltaTime;

		if (!_animStarted && _delay - _elapsedTime < _animationDelay)
		{
			//Debug.Log("paf");
			_animator.SetTrigger("Shoot");
			_animStarted = true;
		}

		if (_elapsedTime >= _delay)
		{
			Shoot();
			_animStarted = false;
			_elapsedTime = 0f;
		}
	}

	public void Shoot()
	{
		//_animator.SetTrigger("Shoot");
		GameObject obj = Instantiate(projectile, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
		obj.GetComponent<Rigidbody>().linearVelocity = obj.transform.forward * projectileSpeed;
	}

	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(startDelay);
		_started = true;
	}

}
