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

	// ?
	Rigidbody rb;

	void Start()
	{
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
		if (InputsManager.Instance.move != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(new Vector3(InputsManager.Instance.move.x, 0f, InputsManager.Instance.move.y), Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3600f * Time.fixedDeltaTime );
		}

		rb.linearVelocity = new Vector3(InputsManager.Instance.move.x * moveSpeed, rb.linearVelocity.y, InputsManager.Instance.move.y * moveSpeed) + dashOffset;
	}

	void Hit()
	{
		if (InputsManager.Instance.hit != HitType.Type.None)
		{
			if (canHit)
				StartCoroutine(ApplyHit(InputsManager.Instance.hit));
			InputsManager.Instance.hit = HitType.Type.None;
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
		PlayerHealth.Instance.SetInvicibility(true);
		gameObject.layer = playerDashingLayer;
		//dashCooldownParticle.Play();

		yield return new WaitForSeconds(dashDuration);
		dashTrail.emitting = false;
		dashOffset = Vector3.zero;
		PlayerHealth.Instance.SetInvicibility(false);
		gameObject.layer = playerLayer;

		yield return new WaitForSeconds(dashCooldown);
        dashTrail.emitting = true;
        canDash = true;
	}
}
