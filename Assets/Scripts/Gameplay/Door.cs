using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
	[SerializeField] float fadeDuration = 1f;

	Material _transparentMaterial;
	Color _baseColor;
	bool _activated = false;
	float _elapsedTime = 0f;

	private void Start()
	{
		_transparentMaterial = GetComponentInChildren<MeshRenderer>().material;
		_baseColor = _transparentMaterial.GetColor("_Base_Color");
	}

	private void Update()
	{
		if (_activated)
		{
			if (_elapsedTime < fadeDuration)
			{
				_baseColor.a = 1f - (_elapsedTime / fadeDuration);

				_transparentMaterial.SetColor("_Base_Color", _baseColor);

				_elapsedTime += Time.deltaTime;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	public void Activate()
	{
		_activated = true;
	}
}
