using UnityEngine;

public class Tomb : MonoBehaviour, IInteractable
{
	[SerializeField, TextArea(4, 10)] string epitaph;
	[SerializeField, TextArea(4, 10)] string memory;

	public void Interact()
	{

	}
}
