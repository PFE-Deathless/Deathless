using System.Collections;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField] Dungeon dungeon;
	[SerializeField] InteractableType interactableType = InteractableType.Interact;
	[SerializeField] float cameraSpeed = 20f;

	[Header("Technical")]
	[SerializeField] IActivable[] toActivateObjects;

	private bool _activated = false;

	public void Interact(InteractableType type = InteractableType.Both)
	{
		if (_activated)
			return;

		if (interactableType == type || interactableType == InteractableType.Both)
		{
			if (GameManager.Instance.HasDungeonSoul(dungeon))
			{
				GameManager.Instance.UnlockDungeonSoul(dungeon);

				// Here goes the logic to change the UI when consuming the dungeon soul

				StartCoroutine(ActivateObjects());

				_activated = true;
			}
		}
	}

	IEnumerator ActivateObjects()
	{
		float elapsedTime;
		Transform start = PlayerController.Instance.transform;
		Transform target = toActivateObjects[0].transform;

		for (int i = 0; i < toActivateObjects.Length - 1; i++)
		{
			elapsedTime = 0f;

			float distance = Vector3.Distance(start.position, target.position);
			float moveDuration = distance / cameraSpeed;

			CameraBehavior.Instance.SetCinematicTarget(target);

			while (elapsedTime < moveDuration)
			{
				target.position = Vector3.Lerp(start.position, target.position, elapsedTime / moveDuration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			while (!toActivateObjects[i].FinishedActivation)
			{

			}

			start = toActivateObjects[i].transform;
			target = toActivateObjects[i + 1].transform;
		}




		yield return null;
	}
}
