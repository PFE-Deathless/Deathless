using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	[SerializeField] HitType.Controller controller;

	[Header("Scene Transition")]
	[SerializeField] string gameScene;
	[SerializeField] string hubScene;
	[SerializeField, Tooltip("Transform to teleport to when coming from a certain level")] HubTransition[] sceneTransitions;

	public string GameScene => gameScene;
	public string HubScene => hubScene;

	public Scene activeScene;


	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		int countLoaded = SceneManager.sceneCount;
		Scene[] loadedScenes = new Scene[countLoaded];

		for (int i = 0; i < countLoaded; i++)
		{
			loadedScenes[i] = SceneManager.GetSceneAt(i);
			Debug.Log("Active scene {" + i + "} : " + loadedScenes[i].name);
		}


		HitType.SetController(controller);
		EnemyBarks.InitBarks();
	}

	public void GoToHub()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByPath(HubScene));
		PlayerController.Instance.Teleport(Vector3.zero, Vector3.zero);
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

[Serializable]
public class HubTransition
{
	public string sceneName;
	public Transform teleportTransform;
}
