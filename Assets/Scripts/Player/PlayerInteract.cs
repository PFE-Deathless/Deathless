using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	readonly List<Transform> interactables = new();

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
