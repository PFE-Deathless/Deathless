using UnityEngine;

public class DoorLock : MonoBehaviour, IInteractable
{
	[SerializeField] InteractableType interactableType = InteractableType.Interact;
	[SerializeField] Door door;

	private bool _activated = false;

	public void Interact(InteractableType type = InteractableType.Both)
	{
		if (_activated)
			return;

		if (interactableType == type || interactableType == InteractableType.Both)
		{
			if (GameManager.Instance.UseKey())
			{
				_activated = true;
				door.Activate();
			}
		}
	}
}
