using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
	[HideInInspector] public static PlayerController Instance { get; private set; }

	[Header("Movement")]
	public float moveSpeed = 10f;
	
	[Header("Hit")]
	public float hitDuration = 0.2f;
	public float hitCooldownSuccess = 0.5f;
	public float hitCooldownFail = 2f;
	public float inputBufferDuration = 0.1f;

	[Header("Dash")]
	public float dashDistance = 5f;
	public float dashDuration = 0.3f;
	public float dashCooldown = 1f;
	public int dashChargesMax = 2;
	public float dashChargesCooldown = 2f;
	public AnimationCurve dashCurve;

	[Header("Technical")]
	public GameObject hitColliderObject;
	public int playerLayer;
	public int playerDashingLayer;

	[Header("VFX")]
	public ParticleSystem dashParticle;
	public ParticleSystem hitCooldownParticle;
	public VisualEffect scytheSlash;
	public SkinnedMeshRenderer scytheRenderer;

	[Header("SFX")]
	[SerializeField] AudioEntry audioScytheSlash;
	[SerializeField] AudioEntry audioDash;

	bool canHit = true;

	// Dash
	bool canDash = true;
	Vector3 dashOffset = Vector3.zero;
	int _dashCharges;
	float _dashChargesElapsedTime = 0f;

	// Hit
	HitCollider hitCollider;
	float _hitElapsedTime;
	bool _isHitting;
	bool _hitSuccess;
	Color _scytheBaseEmissive;

	// Input buffer
	float _inputBufferElapsedTime = 0f;
	HitType.Type _bufferedAttack = HitType.Type.None;

	// ?
	Rigidbody rb;
	Animator animator;
	float animAcceleration = 10f;
	float animCurrentSpeed = 0f;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		hitCollider = hitColliderObject.GetComponent<HitCollider>();
		InputsManager.Instance.hit = HitType.Type.None;
		_dashCharges = dashChargesMax;
		_scytheBaseEmissive = scytheRenderer.material.GetVector("_EmissionColor");

	}

	void Update()
	{
		Interact();

		Hit();

		Dash();
	}

	void FixedUpdate()
	{
		Move();
	}

	void Interact()
	{
		if (InputsManager.Instance.interact)
		{
			// Interact with the nearest possible interactable
			InputsManager.Instance.interact = false;
			PlayerInteract.Instance.Interact();
		}
	}

	void Move()
	{
		if (InputsManager.Instance.move.sqrMagnitude > 0.01f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(new Vector3(InputsManager.Instance.move.x, 0f, InputsManager.Instance.move.y), Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.fixedDeltaTime );
		}

		rb.linearVelocity = new Vector3(InputsManager.Instance.move.x * moveSpeed, rb.linearVelocity.y, InputsManager.Instance.move.y * moveSpeed) + dashOffset;

		if (rb.linearVelocity.sqrMagnitude > 0.01f)
		{
			if (animCurrentSpeed < 1f)
				animCurrentSpeed += Time.fixedDeltaTime * animAcceleration;
			else
				animCurrentSpeed = 1f;
		}
		else
		{
			if (animCurrentSpeed > 0f)
				animCurrentSpeed -= Time.fixedDeltaTime * animAcceleration;
			else
				animCurrentSpeed = 0f;
		}
		
		animator.SetFloat("Speed", animCurrentSpeed);
	}

	void Hit()
	{
		if (InputsManager.Instance.hit != HitType.Type.None)
		{
			_bufferedAttack = InputsManager.Instance.hit;
			_inputBufferElapsedTime = inputBufferDuration;

			InputsManager.Instance.hit = HitType.Type.None;
		}

		if (_inputBufferElapsedTime > 0)
			_inputBufferElapsedTime -= Time.deltaTime;
		else
			_bufferedAttack = HitType.Type.None;

		if (_bufferedAttack != HitType.Type.None && canHit && !_isHitting)
		{
			_isHitting = true;
			canHit = false;
			_hitElapsedTime = 0f;
			hitCollider.SetType(_bufferedAttack);
			scytheSlash.SetInt("HitType", (int)_bufferedAttack);
			hitColliderObject.SetActive(true);
			scytheSlash.Play();
			AudioManager.Instance.Play(audioScytheSlash, transform);
			animator.SetTrigger("Attack");
			scytheRenderer.material.SetVector("_EmissionColor", _scytheBaseEmissive * 0f);
			_bufferedAttack = HitType.Type.None;
		}

		if (_isHitting)
		{
			float cooldownDuration = (_hitSuccess ? hitCooldownSuccess : hitCooldownFail) + hitDuration;

			if (_hitElapsedTime < hitDuration)
			{
				float percentage = (_hitElapsedTime / hitDuration);

				_hitSuccess = hitCollider.HitSucess;
				//HitDisplay.Instance.SetVignPercentage(1f - percentage);
				scytheRenderer.material.SetVector("_EmissionColor", _scytheBaseEmissive * (1f - percentage) * 4f);
			}
			else if (_hitElapsedTime < cooldownDuration)
			{
				float percentage = (_hitElapsedTime - hitDuration) / (cooldownDuration - hitDuration);

				if (_hitSuccess)
					HitDisplay.Instance.SetVignPercentage(1f);
				else
					HitDisplay.Instance.SetVignPercentage(percentage);

				scytheRenderer.material.SetVector("_EmissionColor", _scytheBaseEmissive * percentage * percentage);
				hitColliderObject.SetActive(false);
			}
			else
			{
				HitDisplay.Instance.SetVignPercentage(1f);
				scytheRenderer.material.SetVector("_EmissionColor", _scytheBaseEmissive * 4f);
				if (!_hitSuccess)
					hitCooldownParticle.Play();
				_isHitting = false;
				canHit = true;
			}

			_hitElapsedTime += Time.deltaTime;
		}



	}

	void Dash()
	{
		if (InputsManager.Instance.dash)
		{
			if (canDash && _dashCharges > 0)
			{
				StartCoroutine(ApplyDash());
				_dashCharges--;
				DashDisplay.Instance.SetDashCooldown(_dashCharges, _dashChargesElapsedTime / dashChargesCooldown);
			}
			InputsManager.Instance.dash = false;
		}

		if (_dashCharges < dashChargesMax)
		{
			if (_dashChargesElapsedTime < dashChargesCooldown)
			{
				_dashChargesElapsedTime += Time.deltaTime;
			}
			else
			{
				_dashChargesElapsedTime = 0f;
				DashDisplay.Instance.BlinkColor(_dashCharges);
				_dashCharges++;
			}
			DashDisplay.Instance.SetDashCooldown(_dashCharges, _dashChargesElapsedTime / dashChargesCooldown);
		}
	}

	public void Knockback(Vector3 force)
	{
		rb.AddForce(force, ForceMode.Impulse);
	}

	public void Teleport(Vector3 teleportPosition, Vector3 teleportRotation)
	{
		rb.Move(teleportPosition, Quaternion.Euler(teleportRotation));
		rb.linearVelocity = Vector3.zero;
		CameraBehavior.Instance.Teleport(teleportPosition);
	}

	public void SetSpeedModifier(float modifier, float duration)
	{
		StartCoroutine(ApplySpeedModifier(modifier, duration));
	}

	public void ResetDashCharges()
	{
		_dashCharges = dashChargesMax;
		_dashChargesElapsedTime = 0f;
		DashDisplay.Instance.SetDashCooldown(_dashCharges, 1f);
	}

	IEnumerator ApplySpeedModifier(float modifier, float duration)
	{
		float originalMoveSpeed = moveSpeed;
		moveSpeed *= modifier;
		yield return new WaitForSeconds(duration);
		moveSpeed = originalMoveSpeed;
	}

	IEnumerator ApplyDash()
	{
		canDash = false;
		float elapsedTime = 0f;
		Vector3 direction = transform.forward;


		//dashOffset = transform.forward * (dashDistance / dashDuration);
		//PlayerHealth.Instance.SetInvicibility(true);
		gameObject.layer = playerDashingLayer;
		dashParticle.Play();

		AudioManager.Instance.Play(audioDash, transform);

		while (elapsedTime < dashDuration)
		{
			dashOffset = (dashDistance / dashDuration) * dashCurve.Evaluate(elapsedTime / dashDuration) * direction;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		//yield return new WaitForSeconds(dashDuration);
		dashOffset = Vector3.zero;
		//PlayerHealth.Instance.SetInvicibility(false);
		gameObject.layer = playerLayer;
		dashParticle.Stop();

		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
}
