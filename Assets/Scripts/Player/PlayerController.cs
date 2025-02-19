using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
	[HideInInspector] public static PlayerController Instance { get; private set; }

	[Header("Movement")]
	public float moveSpeed = 10f;
	
	[Header("Hit")]
	public float hitDuration = 0.2f;
	public float hitCooldown = 0.5f;
	public float inputBufferTime = 0.1f;

	[Header("Dash")]
	public float dashDistance = 5f;
	public float dashDuration = 0.3f;
	public float dashCooldown = 1f;
	public AnimationCurve dashCurve;

	[Header("Technical")]
	public GameObject hitCollider;
	public int playerLayer;
	public int playerDashingLayer;

	[Header("VFX")]
	public TrailRenderer dashTrail;
	public ParticleSystem dashCooldownParticle;
	public ParticleSystem dashParticle;
	public VisualEffect scytheSlash;

	bool canHit = true;
	bool canDash = true;

	Vector3 dashOffset = Vector3.zero;

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
		var ps = dashCooldownParticle.main;
		ps.startLifetime = dashCooldown + dashDuration;
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
			Transform t = StaticFunctions.GetNearest(PlayerInteract.Instance.Interactables, transform.position);
			if (t != null)
				t.GetComponent<IInteractable>().Interact();
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
			if (canHit)
				StartCoroutine(ApplyHit(InputsManager.Instance.hit));
			InputsManager.Instance.hit = HitType.Type.None;

			// Interact
			Transform t = StaticFunctions.GetNearest(PlayerInteract.Instance.Interactables, transform.position);
			if (t != null)
				t.GetComponent<IInteractable>().Interact();
		}
	}

	void Dash()
	{
		if (InputsManager.Instance.dash)
		{
			if (canDash)
				StartCoroutine(ApplyDash());
			InputsManager.Instance.dash = false;
		}
	}

	public void Teleport(Vector3 teleportPosition, Vector3 teleportRotation)
	{
		rb.Move(teleportPosition, Quaternion.Euler(teleportRotation));
		CameraBehavior.Instance.Teleport(teleportPosition);
	}

	public void SetSpeedModifier(float modifier, float duration)
	{
		StartCoroutine(ApplySpeedModifier(modifier, duration));
	}

	IEnumerator ApplySpeedModifier(float modifier, float duration)
	{
		float originalMoveSpeed = moveSpeed;
		moveSpeed *= modifier;
		yield return new WaitForSeconds(duration);
		moveSpeed = originalMoveSpeed;
	}

	IEnumerator ApplyHit(HitType.Type type)
	{
		canHit = false;

		hitCollider.SetActive(true);
		hitCollider.GetComponent<HitCollider>().SetType(type);
		scytheSlash.SetInt("HitType", (int)type - 1);
		scytheSlash.Play();
		animator.SetTrigger("Attack");

		yield return new WaitForSeconds(hitDuration);

		hitCollider.SetActive(false);

		yield return new WaitForSeconds(hitCooldown);
		canHit = true;
	}

	IEnumerator ApplyDash()
	{
		canDash = false;
		float elapsedTime = 0f;
		Vector3 direction = transform.forward;


		//dashOffset = transform.forward * (dashDistance / dashDuration);
		PlayerHealth.Instance.SetInvicibility(true);
		gameObject.layer = playerDashingLayer;
		dashParticle.Play();



		while (elapsedTime < dashDuration)
		{
			dashOffset = (dashDistance / dashDuration) * dashCurve.Evaluate(elapsedTime / dashDuration) * direction;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		//yield return new WaitForSeconds(dashDuration);
		dashTrail.emitting = false;
		dashOffset = Vector3.zero;
		PlayerHealth.Instance.SetInvicibility(false);
		gameObject.layer = playerLayer;
		dashParticle.Stop();

		yield return new WaitForSeconds(dashCooldown);
		dashTrail.emitting = true;
		canDash = true;
	}
}
