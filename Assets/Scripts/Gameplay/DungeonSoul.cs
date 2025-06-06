using UnityEngine;

public class DungeonSoul : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField, Tooltip("Type of interaction the player need to do to take the soul")] InteractableType interactableType = InteractableType.Hit;
	[SerializeField, Tooltip("Dungeon that this soul will validate after being reaped by the player")] Dungeon dungeonValidation;
	[SerializeField, ColorUsage(false, true)] Color soulColor;

	[Header("Technical")]
	[SerializeField, Tooltip("Door that will be opened after the soul is reaped")] Door door;

	private void Start()
	{
		GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", soulColor);

		if (GameManager.Instance.IsUnlocked(dungeonValidation))
		{
			if (door != null)
				door.Activate();
			Destroy(gameObject);
		}
	}

	public void Interact(InteractableType type)
	{
		if (interactableType != type && interactableType != InteractableType.Both)
			return;

		if (door != null)
			door.Activate();
		GameManager.Instance.UnlockDungeonSoul(dungeonValidation);
		PlayerInteract.Instance.Remove(transform);
		Destroy(gameObject);
	}
}
