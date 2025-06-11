using UnityEngine;

public class CemeteryDoor : MonoBehaviour, IActivable
{
	[Header("Properties")]
	[SerializeField] Dungeon dungeon = Dungeon.Tutorial;
	[SerializeField] float openingDuration = 1f;

	[Header("Doors References")]
	[SerializeField] Transform doorLeft;
	[SerializeField] Transform doorRight;

	[Header("Left Door")]
	[SerializeField] float leftStartAngle;
	[SerializeField] float leftEndAngle;

	[Header("Right Door")]
	[SerializeField] float rightStartAngle;
	[SerializeField] float rightEndAngle;

	private bool _activated = false;
	private float _elapsedTime = 0f;

	public bool FinishedActivation { get; set; }

	private void Start()
	{
		if (GameManager.Instance.IsUnlocked(dungeon))
			_activated = true;
	}

	private void Update()
	{
		if (_activated)
		{
			if (_elapsedTime < openingDuration)
			{
				float t = _elapsedTime / openingDuration;

				doorLeft.localRotation = Quaternion.Lerp(Quaternion.AngleAxis(leftStartAngle, Vector3.up), Quaternion.AngleAxis(leftEndAngle, Vector3.up), t);
				doorRight.localRotation = Quaternion.Lerp(Quaternion.AngleAxis(rightStartAngle, Vector3.up), Quaternion.AngleAxis(rightEndAngle, Vector3.up), t);

				_elapsedTime += Time.deltaTime;
			}
			else
			{
				FinishedActivation = true;
			}
		}
	}

	public void Activate(bool playAnimation = true)
	{
		_activated = true;
	}
}
