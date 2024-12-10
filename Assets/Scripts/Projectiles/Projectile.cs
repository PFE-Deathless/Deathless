using UnityEngine;

public class Projectile : MonoBehaviour
{
	Rigidbody rb;
	Transform target;

	public void Setup(float speed, Transform target)
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		this.target = target;
		rb.linearVelocity = transform.forward * speed;
	}
}
