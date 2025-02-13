using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	[HideInInspector] public static PlayerInteract Instance { get; private set; }

	readonly List<Transform> interactables = new();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
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
