public interface IInteractable
{
	public void Interact(InteractableType type = InteractableType.Both);
}

public enum InteractableType
{
	Interact,
	Hit,
	Both
}