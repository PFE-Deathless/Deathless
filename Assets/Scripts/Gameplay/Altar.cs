using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		PlayerSouls.Instance.ValidateSouls();
	}
}
