using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
	[Header("Parameters")]
	[SerializeField] float fadeDuration = 1f;
	[SerializeField, Tooltip("Type of door (Lever : opened by a lever, Progression : Opened by a finished dungeon, Key : Opened by a key)")] Type doorType = Type.Lever;
	[SerializeField, Tooltip("Dungeon that opens the door if finished (If Progression is selected in Door Type)")] Dungeon dungeonValidation;

	Material _transparentMaterial;
	Color _baseColor;
	bool _activated = false;
	float _elapsedTime = 0f;

	private void Start()
	{
		if (doorType == Type.Progression && GameManager.Instance.IsUnlocked(dungeonValidation))
			Destroy(gameObject);

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

	public enum Type
	{
		Lever,
		Progression,
		Key,
	}
}
