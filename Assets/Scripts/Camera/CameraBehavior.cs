using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CameraBehavior : MonoBehaviour
{
	[HideInInspector] public static CameraBehavior Instance { get; private set; }

	[Header("Camera Follow")]
	[SerializeField] Vector3 offset = new(0f, 40f, -20f);
	[SerializeField] float smoothDampTime = 0.3f;
	[SerializeField] Transform cameraTransform;

	[Header("Camera Transparency")]
	[SerializeField] string ditheredShaderName = "Shader Graphs/S_DitherTransparency";
	[SerializeField, Range(0f, 1f)] float transparentPercentage = 0.5f;
	[SerializeField] float transparentRadius = 2f;

	private Transform playerTransform;
	private Vector3 currentVelocity;
	private List<ShakeInstance> activeShakes = new List<ShakeInstance>();

	// Transparent walls
	private Collider[] _transparentColliders = new Collider[5];
	private RaycastHit[] _transparentHits = new RaycastHit[5];
	private List<MeshRenderer> _transparentMR = new List<MeshRenderer>();
	private List<MeshRenderer> _transparentActiveMR = new List<MeshRenderer>();
	private List<MeshRenderer> _transparentUnactiveMR = new List<MeshRenderer>();

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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			Shake(0.4f, 20f, 0.5f);
		}
	}

	void LateUpdate()
	{
		ManageTransparentDecors();

		ManageShakes();

		Follow();
	}

	void ManageTransparentDecors()
	{
		//int transparentObjectCount = Physics.OverlapCapsuleNonAlloc(transform.position, playerTransform.position, transparentRadius, _transparentColliders);

		int transparentObjectCount = Physics.RaycastNonAlloc(playerTransform.position, transform.position - playerTransform.position, _transparentHits, 5f);

		_transparentMR.Clear();
		if (transparentObjectCount > 0)
		{
			for (int i = 0; i < transparentObjectCount; i++)
			{
				MeshRenderer mr = _transparentHits[i].collider.GetComponentInChildren<MeshRenderer>();
				if (mr != null && mr.material.shader.name == ditheredShaderName)
					_transparentMR.Add(mr);
			}
				
		}

		for (int i = 0; i < _transparentMR.Count; i++)
		{
			if (!_transparentActiveMR.Contains(_transparentMR[i]))
			{
				Color c = _transparentMR[i].material.GetColor("_Base_Color");
				c.a = transparentPercentage;
				_transparentMR[i].material.SetColor("_Base_Color", c);
				_transparentActiveMR.Add(_transparentMR[i]);
			}
		}

		for (int i = 0; i < _transparentActiveMR.Count; i++)
		{
			if (!_transparentMR.Contains(_transparentActiveMR[i]))
			{
				_transparentUnactiveMR.Add(_transparentActiveMR[i]);
			}
		}

		for (int i = 0; i < _transparentUnactiveMR.Count; i++)
		{
			Color c = _transparentUnactiveMR[i].material.GetColor("_Base_Color");
			c.a = 1f;
			_transparentUnactiveMR[i].material.SetColor("_Base_Color", c);
			_transparentActiveMR.Remove(_transparentUnactiveMR[i]);
		}

		_transparentUnactiveMR.Clear();






		//if (transparentObjectCount > 0)
		//{
		//	for (int i = 0; i < j; i++)
		//	{
		//		//Debug.Log($"{i} : {transparentColliders[i].name}");

		//		MeshRenderer mr = transparentColliders[i].GetComponentInChildren<MeshRenderer>();
		//		if (mr != null && mr.material.shader.name == ditheredShaderName)
		//		{
		//			if (!transparentMeshRenderers.Contains(mr))
		//			{
		//				Color c = mr.material.GetColor("_Base_Color");
		//				c.a = transparentPercentage;
		//				mr.material.SetColor("_Base_Color", c);
		//				transparentMeshRenderers.Add(mr);
		//			}


		//			//Debug.Log("Shader name : " + mr.material.shader.name);
		//			//Color c = mr.material.GetColor("_Base_Color");
		//			//c.a = 0.5f;
		//			//mr.material.SetColor("_Base_Color", c);
		//			//Destroy(mr.gameObject);
		//		}
		//	}

		//	//Debug.Log("j : " + j);transparentMeshRenderers
		//}

		//for (int i = 0; i < transparentMeshRenderers.Count; i++)
		//{
		//	if (!Array.Exists(transparentColliders, x => x == transparentMeshRenderers[i].gameObject.GetComponent<Collider>()))
		//	{
		//		Debug.Log("ayaya : " + transparentMeshRenderers[i].name);
		//		Color c = transparentMeshRenderers[i].material.GetColor("_Base_Color");
		//		c.a = 1f;
		//		transparentMeshRenderers[i].material.SetColor("_Base_Color", c);
		//		transparentMRtoDelete.Add(transparentMeshRenderers[i]);
		//	}
		//}

		//if (transparentMRtoDelete.Count > 0)
		//{
		//	foreach (MeshRenderer mr in transparentMRtoDelete)
		//		transparentMeshRenderers.Remove(mr);
		//	transparentMRtoDelete.Clear();
		//}
	}

	void Follow()
	{
		transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position + offset, ref currentVelocity, smoothDampTime);
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

		_transparentUnactiveMR.Clear();
		_transparentActiveMR.Clear();
	}

	public void Shake(float amplitude, float frequency, float duration)
	{
		activeShakes.Add(new ShakeInstance(amplitude, frequency, duration));
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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(transform.position, transparentRadius);
		Gizmos.DrawWireSphere(playerTransform.position, transparentRadius);
		Gizmos.DrawLine(transform.position, playerTransform.position);
	}
}
