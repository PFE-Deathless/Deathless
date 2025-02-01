using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	[HideInInspector] public static CameraBehavior Instance {  get; private set; }

	[Header("Camera Follow")]
	public Vector3 offset;
	[Range(0.00001f, 179)] public float fov = 20f;
	public float smoothDampTime;

	Vector3 currentVelocity;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

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

	public void Teleport(Vector3 teleportPosition)
	{
		currentVelocity = Vector3.zero;
		Camera.main.transform.position = teleportPosition + offset;
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
