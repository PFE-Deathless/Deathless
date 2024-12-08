using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Statistics")]
	public float moveSpeed = 10f;
	public float hitDuration = 0.2f;
	public float hitCooldown = 0.5f;

	[Header("Technical")]
	public GameObject hitCollider;


	bool canHit = true;

	InputsManager inputManager;
	Rigidbody rb;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		inputManager = GetComponent<InputsManager>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();

		Hit();
	}

	void Move()
	{
		if (inputManager.move != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputManager.move.x, 0f, inputManager.move.y), Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20f);
		}


		rb.linearVelocity = new Vector3(inputManager.move.x * moveSpeed, rb.linearVelocity.y, inputManager.move.y * moveSpeed);
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
}
