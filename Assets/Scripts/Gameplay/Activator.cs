using TMPro;
using UnityEngine;

public class Activator : MonoBehaviour, IInteractable
{
	public Door activable;
	public InteractableType interactableType = InteractableType.Both;
	public TextMeshPro tmp;
	//private bool inBox = false;
	private bool canInteract = true;
	public GameObject levier;
	public GameObject _feedback;

	void Start()
	{
		tmp.enabled = false;
	}

	public void Interact(InteractableType type)
	{
		if (canInteract && (interactableType == type || interactableType == InteractableType.Both))
		{
			activable.Activate();

			// feedback
			levier.transform.localRotation = Quaternion.Euler(0f, 0f, 30f);
			ParticleSystem ps = _feedback.GetComponent<ParticleSystem>();
			ps.Play();

			// l'objet n'est plus utilisable
			canInteract = false;
		}
	}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.layer == 3 && canInteract)
	//	{
	//		inBox = true;
	//		tmp.enabled = true;
	//	}
	//}

	//private void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.layer == 3)
	//	{
	//		tmp.enabled = false;
	//		inBox = false;
	//	}
	//}
}
