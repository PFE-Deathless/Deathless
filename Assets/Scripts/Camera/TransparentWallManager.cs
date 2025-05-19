using System.Collections.Generic;
using UnityEngine;

public class TransparentWallManager : MonoBehaviour
{
	[Header("Camera Transparency")]
	[SerializeField] string ditheredShaderName = "Shader Graphs/S_DitherTransparency";
	[SerializeField, Range(0f, 1f)] float transparentPercentage = 0.5f;
	[SerializeField] float fadeDuration = 0.2f;

	[Header("Technical")]
	[SerializeField] Transform cameraTransform;
	[SerializeField] LayerMask wallLayer = (1 << 18);

	private Transform _playerTransform;
	private Rigidbody rb;

	[SerializeField]  private List<MeshRenderer> _inMR = new();
	[SerializeField]  private List<MeshRenderer> _outMR = new();
	[SerializeField]  private List<MeshRenderer> _clearMR = new();

	private void Start()
	{
		_playerTransform = PlayerController.Instance.transform;
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (_playerTransform != null)
		{
			Vector3 targetPos = _playerTransform.position + new Vector3(0f, 0f, -0.4f);
			Vector3 direction = targetPos - cameraTransform.position;

			rb.Move(Vector3.Lerp(targetPos, cameraTransform.position, 0.15f), Quaternion.LookRotation(direction));
		}

		ManageTransparency();
	}

	void ManageTransparency()
	{
		if (GameManager.Instance.LevelIsLoading)
		{
			if (_inMR.Count > 0)
			{
				foreach (MeshRenderer mr in _inMR)
					mr.material.SetColor("_Base_Color", Color.white);
				_inMR.Clear();
			}

			if (_outMR.Count > 0)
			{
				foreach (MeshRenderer mr in _outMR)
					mr.material.SetColor("_Base_Color", Color.white);
				_outMR.Clear();
			}

			if (_clearMR.Count > 0)
				_clearMR.Clear();

			return;
		}

		// Fade hit walls to transparent
		foreach (MeshRenderer mr in _inMR)
		{
			if (mr == null)
			{
				if (!_clearMR.Contains(mr))
					_clearMR.Add(mr);
				continue;
			}

			Color c = mr.material.GetColor("_Base_Color");

			if (c.a > transparentPercentage)
			{
				c.a -= (1f - transparentPercentage) * (Time.deltaTime / fadeDuration);
				mr.material.SetColor("_Base_Color", c);
			}
			else
			{
				c.a = transparentPercentage;
				mr.material.SetColor("_Base_Color", c);
			}
		}

		// Fade the others back to opaque
		foreach (MeshRenderer mr in _outMR)
		{
			if (mr == null)
			{
				if (!_clearMR.Contains(mr))
					_clearMR.Add(mr);
				continue;
			}

			Color c = mr.material.GetColor("_Base_Color");

			if (c.a < 1f)
			{
				c.a += (1f - transparentPercentage) * (Time.deltaTime / fadeDuration);
				mr.material.SetColor("_Base_Color", c);
			}
			else
			{
				c.a = 1f;
				mr.material.SetColor("_Base_Color", c);
				if (!_clearMR.Contains(mr))
					_clearMR.Add(mr);
			}

		}

		foreach (MeshRenderer mr in _clearMR)
		{
			_outMR.Remove(mr);
			_inMR.Remove(mr);
		}
		_clearMR.Clear();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (GameManager.Instance.LevelIsLoading)
			return;

		if ((wallLayer.value & (1 << other.gameObject.layer)) > 0)
		{
			MeshRenderer mr = other.GetComponentInChildren<MeshRenderer>();

			if (mr == null || mr.material.shader.name != ditheredShaderName)
				return;

			if (!_inMR.Contains(mr))
				_inMR.Add(mr);

			if (_outMR.Contains(mr))
				_outMR.Remove(mr);

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (GameManager.Instance.LevelIsLoading)
			return;

		if ((wallLayer.value & (1 << other.gameObject.layer)) > 0)
		{
			MeshRenderer mr = other.GetComponentInChildren<MeshRenderer>();

			if (mr == null || mr.material.shader.name != ditheredShaderName)
				return;

			if (!_outMR.Contains(mr))
				_outMR.Add(mr);

			if (_inMR.Contains(mr))
				_inMR.Remove(mr);

		}
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;

	//	Vector3 direction = (_playerTransform.position - cameraTransform.position);
	//	Gizmos.DrawRay(cameraTransform.position, direction);
	//}
}
