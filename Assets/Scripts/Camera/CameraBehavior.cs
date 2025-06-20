using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	[HideInInspector] public static CameraBehavior Instance { get; private set; }

	[Header("Camera Follow")]
	[SerializeField] Vector3 offset = new(0f, 40f, -20f);
	[SerializeField] float smoothDampTime = 0.3f;
	[SerializeField] Transform cameraTransform;

	private Transform playerTransform;
	private Vector3 currentVelocity;
	private List<ShakeInstance> activeShakes = new List<ShakeInstance>();

	private bool _cinematic;
	private Transform _cinematicTargetTransform;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	void Start()
	{
		playerTransform = PlayerController.Instance.transform;
		transform.position = playerTransform.position + offset;
		transform.LookAt(playerTransform.position);
	}

	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.O))
	//	{
	//		Shake(0.4f, 20f, 0.5f);
	//	}
	//}

	void LateUpdate()
	{
		//ManageTransparentDecors();

		ManageShakes();

		Follow(_cinematic ? _cinematicTargetTransform.position : playerTransform.position);
	}

	void Follow(Vector3 targetPos)
	{
		transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset, ref currentVelocity, smoothDampTime);
	}

	void ManageShakes()
	{
		Vector3 totalShakeOffset = Vector3.zero;

		for (int i = activeShakes.Count - 1; i >= 0; i--)
		{
			ShakeInstance shake = activeShakes[i];

			float strength = shake.GetCurrentStrength();
			float shakeX = (Mathf.PerlinNoise(Time.time * shake.frequency, 0) - 0.5f) * 2f * strength;
			float shakeY = (Mathf.PerlinNoise(0, Time.time * shake.frequency) - 0.5f) * 2f * strength;

			totalShakeOffset += new Vector3(shakeX, shakeY, 0);

			// Reduce shake duration
			shake.timeLeft -= Time.deltaTime;
			if (shake.timeLeft <= 0)
			{
				activeShakes.RemoveAt(i);
			}
		}

		cameraTransform.localPosition = totalShakeOffset;
	}

	public void Teleport(Vector3 teleportPosition)
	{
		transform.position = teleportPosition + offset;
		currentVelocity = Vector3.zero;
	}

	public void Shake(float amplitude, float frequency, float duration)
	{
		//Debug.Log("Shake !");
		activeShakes.Add(new ShakeInstance(amplitude, frequency, duration));
	}

	public void SetCinematicTarget(Transform target)
	{
		if (target == null)
		{
			_cinematic = false;
		}
		else
		{
			_cinematicTargetTransform = target;
			_cinematic = true;
		}
	}

	private class ShakeInstance
	{
		public float amplitude;
		public float frequency;
		public float duration;
		public float timeLeft;

		public ShakeInstance(float amplitude, float frequency, float duration)
		{
			this.amplitude = amplitude;
			this.frequency = frequency;
			this.duration = duration;
			this.timeLeft = duration;
		}

		public float GetCurrentStrength()
		{
			float t = timeLeft / duration;
			return amplitude * t * t; // Quadratic easing (faster falloff)
		}
	}
}
