using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	[SerializeField] HitType.Controller controller;

	[Header("Scene Transitions")]
	[SerializeField] string gameScene;
	[SerializeField] string hubScene;
	
	public string GameScene => gameScene;
	public string HubScene => hubScene;

	public Scene activeScene;

	Transform _transformToTeleportTo;


	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		//int countLoaded = SceneManager.sceneCount;
		//Scene[] loadedScenes = new Scene[countLoaded];

		//for (int i = 0; i < countLoaded; i++)
		//{
		//	loadedScenes[i] = SceneManager.GetSceneAt(i);
		//	Debug.Log("Active scene {" + i + "} : " + loadedScenes[i].name);
		//}

		HitType.SetController(controller);
		EnemyBarks.InitBarks();
	}

	// Method to call when the player comes from a level to the hub to know where to tp him
	public void GoToHub()
	{
		//Debug.Log("SCENE : " + activeScene.path);

		SceneManager.SetActiveScene(SceneManager.GetSceneByPath(HubScene));
		Transform t = SceneHubStart.Instance.GetTransformFromPath(activeScene.path);
		PlayerController.Instance.Teleport(t.position, t.eulerAngles);
		SceneManager.UnloadSceneAsync(activeScene);
	}

	private void Update()
	{
		if (InputsManager.Instance.reloadScene)
		{
			InputsManager.Instance.reloadScene = false;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
