using System;
using UnityEngine;

public class SceneHubStart : MonoBehaviour
{
	[HideInInspector] public static SceneHubStart Instance { get; private set; }

	[SerializeField] Transform defaultTransform;
	[SerializeField, Tooltip("Transform to teleport to when coming to the hub from a certain level")] HubTransition[] sceneTransitions;


	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public Transform GetTransformFromPath(string path)
	{
		for (int i = 0; i < sceneTransitions.Length; i++)
		{
			if (sceneTransitions[i].sceneName == path)
				return sceneTransitions[i].teleportTransform;
		}
		return defaultTransform;
	}
}



[Serializable]
public class HubTransition
{
	public string sceneName;
	public Transform teleportTransform;

	
}
