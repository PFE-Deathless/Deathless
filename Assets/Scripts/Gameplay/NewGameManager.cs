using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{
	public static NewGameManager Instance { get; private set; }

	[Header("Objects to spawn on level start")]
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject userInterfacePrefab;
	[SerializeField] GameObject cameraPrefab;
	[SerializeField, Tooltip("Transform the player objects will be attached to")] Transform playerParent;

	[Header("Scene Transition")]
	[SerializeField] string loadingScreenScenePath;
	[SerializeField] float fadeInDuration = 0.5f;
	[SerializeField] float fadeOutDuration = 0.5f;
	[SerializeField] float loadingScreenDuration = 1f;

	[Header("Projectiles")]
	[SerializeField, Tooltip("Transform the projectiles will be attached to")] Transform projectileParent;

	[Header("Controller type")]
	[SerializeField] HitType.Controller controller;

	public Transform ProjectileParent => projectileParent;

	// Private properties
	bool _loadingLevel = false;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);

		HitType.SetController(controller);
		EnemyBarks.InitBarks();
		SpawnPlayer();
	}

	void SpawnPlayer()
	{
		Transform playerStartPosition = null;
		while (playerStartPosition == null)
			playerStartPosition = GameObject.FindGameObjectWithTag("BeginPlay").transform;

		Instantiate(userInterfacePrefab, playerParent);
		Instantiate(playerPrefab, playerStartPosition.position, playerStartPosition.rotation, playerParent);
		Instantiate(cameraPrefab, playerStartPosition.position, Quaternion.identity, playerParent);
	}

	void DestroyPlayer()
	{
		foreach (Transform t in playerParent)
			Destroy(t.gameObject);
	}

	public void LoadLevel(string scenePath)
	{
		if (!_loadingLevel)
			StartCoroutine(LoadLevelCoroutine(scenePath));
	}

	IEnumerator LoadLevelCoroutine(string scenePath)
	{
		_loadingLevel = true;

		// Load loading screen scene
		SceneManager.LoadSceneAsync(loadingScreenScenePath, LoadSceneMode.Additive);
		Scene loadingScreenScene = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);

		// Wait until the loading screen scene to be loaded
		while (LoadingScreen.Instance == null)
			yield return null;

		// Block player inputs
		InputsManager.Instance.canInput = false;

		// Start fade in
		LoadingScreen.Instance.SetTiming(fadeInDuration, fadeOutDuration);
		LoadingScreen.Instance.FadeIn();

		// Get current scene
		Scene oldLevel = SceneManager.GetActiveScene();

		//Debug.Log("Scene : " + loadingScreenScene.path);
		//Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);

		// Wait for the fade in to finish
		while (LoadingScreen.Instance.IsFadingIn)
			yield return null;

		// Start loading the new level
		AsyncOperation newLevelAO = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
		newLevelAO.allowSceneActivation = false;


		// Loading in progress
		while (newLevelAO.progress < 0.9f && !newLevelAO.isDone)
		{
			// Loading Screen progress here

			//LoadingScreen.Instance.SetProgressBarValue(newLevelAO.progress / 0.9f);
			yield return null;
		}
		
		// Activate new level
		newLevelAO.allowSceneActivation = true;

		//// Wait for the level to be fully loaded
		//while (!newLevelAO.isDone)
		//{
		//	//Debug.Log("in progress");
		//	yield return null;
		//}


		// Wait for the load screen to do its things uh
		yield return new WaitForSeconds(loadingScreenDuration);

		// Set new level as active level
		Scene newLevel = SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
		SceneManager.SetActiveScene(newLevel);
		//Debug.Log("New Level : " + newLevel.path);

		// Teleport player
		Transform playerTransform = GameObject.FindGameObjectWithTag("BeginPlay").transform;
		PlayerController.Instance.Teleport(playerTransform.position, playerTransform.eulerAngles);

		// Start fade in
		LoadingScreen.Instance.FadeOut();

		// Unload previous level
		SceneManager.UnloadSceneAsync(oldLevel);

		// Wait for the fade out to finish
		while (LoadingScreen.Instance.IsFadingOut)
			yield return null;

		// Unload loading screen level
		SceneManager.UnloadSceneAsync(loadingScreenScene);

		// Wait a bit and activate player inputs back
		yield return new WaitForSeconds(0.2f);
		InputsManager.Instance.canInput = true;

		//Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);


		//Debug.Log("Nb Scene : " + SceneManager.loadedSceneCount);
		//for (int i = 0; i < SceneManager.loadedSceneCount; i++)
		//{
		//	Debug.Log($"Scene ({i}) : {SceneManager.GetSceneAt(i).path}");
		//}


		_loadingLevel = false;

		yield return null;
	}
}
