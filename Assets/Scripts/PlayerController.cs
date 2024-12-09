using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 10f;
	
	[Header("Hit")]
	public float hitDuration = 0.2f;
	public float hitCooldown = 0.5f;

	[Header("Dash")]
	public float dashDistance = 5f;
	public float dashDuration = 0.3f;
	public float dashCooldown = 1f;

	[Header("Technical")]
	public GameObject hitCollider;


	bool canHit = true;
	bool canDash = true;

	Vector3 dashOffset = Vector3.zero;

	InputsManager inputManager;
	PlayerHealth playerHealth;
	Rigidbody rb;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		inputManager = GetComponent<InputsManager>();
		playerHealth = GetComponent<PlayerHealth>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();

		Hit();

		Dash();
	}

	void Move()
	{
		if (inputManager.move != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputManager.move.x, 0f, inputManager.move.y), Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20f);
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
		
		yield return new WaitForSeconds(dashDuration);
		dashOffset = Vector3.zero;
		playerHealth.SetInvicibility(false);

		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
}
