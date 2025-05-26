using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	public static PlayerInteract Instance { get; private set; }

	[SerializeField] GameObject interactObjectPrefab;

	[SerializeField] List<Transform> interactables = new();
	IInteractable _nearest;

	GameObject _interactObject;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Update()
	{
		SetInteractObject();
	}

	public void Interact()
	{
		if (interactables.Count > 0 && _nearest != null)
		{
			_nearest.Interact(InteractableType.Interact);
		}
	}

	public void Remove(Transform interactable)
	{
		interactables.Remove(interactable);
	}

	void SetInteractObject()
	{
		if (interactables.Count == 0)
		{
			if (_interactObject != null)
				_interactObject.SetActive(false);
			_nearest = null;
			return;
		}

		if (_interactObject == null)
			_interactObject = Instantiate(interactObjectPrefab, transform); 

		_interactObject.SetActive(true);

		Transform nearest = StaticFunctions.GetNearest(interactables, transform.position);

		if (nearest == null)
		{
			_interactObject.SetActive(false);
			return;
		}
		else
		{
			if (nearest.TryGetComponent(out IInteractable interact))
				_nearest = interact;
			else
				return;
		}

		_interactObject.transform.position = nearest.position;
	}

	public void ClearInteract()
	{
		interactables.Clear();
	}

	public List<Transform> Interactables
	{
		get { return interactables; }
		set { }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!interactables.Contains(other.transform))
		{
			interactables.Add(other.transform);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		interactables.Remove(other.transform);
	}
}
