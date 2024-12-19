using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	[Header("Camera Follow")]
	public Transform objToFollow;
	public Vector3 offset;
	public float smoothDampTime;

	Vector3 currentVelocity;

	void Start()
	{
        transform.position = objToFollow.position + offset;
        transform.LookAt(objToFollow.position);
	}

	void LateUpdate()
	{
		Follow();
	}

	void Follow()
	{
		transform.position = Vector3.SmoothDamp(transform.position, objToFollow.position + offset, ref currentVelocity, smoothDampTime);
	}
}
