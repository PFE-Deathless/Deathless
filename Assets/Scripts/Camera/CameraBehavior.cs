using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	[Header("Camera Follow")]
	public Vector3 offset;
	[Range(0.00001f, 179)] public float fov = 20f;
	public float smoothDampTime;

	Vector3 currentVelocity;

	void Start()
	{
		Camera.main.transform.position = transform.position + offset;
		Camera.main.transform.LookAt(transform.position);
	}

	void LateUpdate()
	{
		Follow();
	}

	void Follow()
	{
		Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, transform.position + offset, ref currentVelocity, smoothDampTime);
	}

	private void OnValidate()
	{
		if (Camera.main != null && transform != null)
		{
			Camera.main.transform.position = transform.position + offset;
			Camera.main.transform.LookAt(transform.position);
			Camera.main.fieldOfView = fov;
		}
	}
}
