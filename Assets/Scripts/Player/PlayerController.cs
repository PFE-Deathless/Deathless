using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

	[Header("Technical")]
	public GameObject hitCollider;
	public int playerLayer;
	public int playerDashingLayer;

	[Header("VFX")]
	public TrailRenderer dashTrail;
	public ParticleSystem dashCooldownParticle;

	bool canHit = true;
	bool canDash = true;

	Vector3 dashOffset = Vector3.zero;


	// Timers
	//float inputBufferTimer = 0f;
	//float hitTimer = 0f;

	// ?
	InputsManager inputManager;
	//HitType.Type inputBuffer = HitType.Type.None;
	//HitType.Type inputCurrent;
	PlayerHealth playerHealth;
	PlayerInteract playerInteract;
	Rigidbody rb;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		inputManager = GetComponent<InputsManager>();
		playerHealth = GetComponent<PlayerHealth>();
		playerInteract = GetComponentInChildren<PlayerInteract>();
		rb = GetComponent<Rigidbody>();
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
		if (inputManager.interact)
		{
			// Interact with the nearest possible interactable
			inputManager.interact = false;
			Transform t = StaticFunctions.GetNearest(playerInteract.Interactables, transform.position);
			if (t != null)
				t.GetComponent<IInteractable>().Interact();
		}
	}

	void Move()
	{
		if (inputManager.move != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputManager.move.x, 0f, inputManager.move.y), Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3600f * Time.fixedDeltaTime );
		}

		rb.linearVelocity = new Vector3(inputManager.move.x * moveSpeed, rb.linearVelocity.y, inputManager.move.y * moveSpeed) + dashOffset;
	}

	void Hit()
	{
		if (inputManager.hit != HitType.Type.None)
		{
			if (canHit)
				StartCoroutine(ApplyHit(inputManager.hit));
			inputManager.hit = HitType.Type.None;
		}

		//if (inputCurrent != HitType.Type.None)
		//{
		//	if (hitTimer <= hitDuration)
		//	{
		//		hitCollider.SetActive(true);
		//		hitCollider.GetComponent<HitCollider>().SetType(inputCurrent);
		//	}
		//	else if (hitTimer > hitDuration && hitTimer <= hitCooldown)
		//	{
		//		hitCollider.SetActive(false);
		//	}
		//	else if (hitTimer > (hitDuration + hitCooldown) - inputBufferTime)
		//	{
		//		inputBuffer = inputManager.hit;
		//	}
		//}

		//if (hitTimer <= hitDuration + hitCooldown && inputCurrent != HitType.Type.None)
		//{
		//	hitTimer += Time.deltaTime;
		//}
		//else
		//{
		//	hitTimer = 0f;
		//	if (inputBuffer != HitType.Type.None)
		//	{
		//		inputCurrent = inputBuffer;
		//		inputBuffer = HitType.Type.None;
		//	}
		//	else
		//		inputCurrent = inputManager.hit;
		//}
	}

	void Dash()
	{
		if (inputManager.dash)
		{
			if (canDash)
				StartCoroutine(ApplyDash());
			inputManager.dash = false;
		}
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

		yield return new WaitForSeconds(hitDuration);

		hitCollider.SetActive(false);

		yield return new WaitForSeconds(hitCooldown);
		canHit = true;
	}

	IEnumerator ApplyDash()
	{
		canDash = false;
		
		dashOffset = transform.forward * (dashDistance / dashDuration);
		playerHealth.SetInvicibility(true);
		gameObject.layer = playerDashingLayer;
		dashTrail.emitting = true;
		dashCooldownParticle.Play();

		yield return new WaitForSeconds(dashDuration);
		dashOffset = Vector3.zero;
		playerHealth.SetInvicibility(false);
		gameObject.layer = playerLayer;
		dashTrail.emitting = false;

		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
}
