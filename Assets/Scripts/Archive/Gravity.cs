using UnityEngine;

public class Gravity : MonoBehaviour
{
	public float gravity = 50f;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	void Update()
	{
		rb.AddForce(new Vector3(0f, -gravity, 0f), ForceMode.Acceleration);
	}
}
