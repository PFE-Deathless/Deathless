using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Objects to spawn on level start")]
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject userInterfacePrefab;
	[SerializeField] GameObject cameraPrefab;
	[SerializeField, Tooltip("Transform the player objects will be attached to")] Transform playerParent;

	[Header("Scene Transition")]
	[SerializeField] string loadingScreenScenePath = "Assets/Scenes/LoadingScreen.unity";
	[SerializeField] float fadeInDuration = 0.5f;
	[SerializeField] float fadeOutDuration = 0.5f;
	[SerializeField] float loadingScreenDuration = 1f;

	[Header("Menu Scene")]
	[SerializeField] string mainMenuScenePath;
	[SerializeField] string[] menuScenePaths;

	[Header("Projectiles")]
	[SerializeField, Tooltip("Transform the projectiles will be attached to")] Transform projectileParent;

	public Transform ProjectileParent => projectileParent;


	// Private properties
	bool _loadingLevel = false;

	// Public attributes
	public bool LevelIsLoading => _loadingLevel;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	private void Start()
	{
#if !UNITY_EDITOR
		Cursor.visible = false;
#endif

		if (!IsMenu(SceneManager.GetActiveScene().path))
		{
			SpawnTeleportPlayer();
		}
	}

	private void Update()
	{
		if (InputsManager.Instance != null && InputsManager.Instance.reloadScene)
		{
			InputsManager.Instance.reloadScene = false;
			ReloadLevel();
		}

		if (InputsManager.Instance != null && InputsManager.Instance.mainMenu)
		{
			InputsManager.Instance.mainMenu = false;
			LoadLevel(mainMenuScenePath);
		}
	}

	bool IsMenu(string path)
	{
		if (menuScenePaths.Length == 0)
			return false;

		int i = 0;

		while (i < menuScenePaths.Length)
		{
			if (menuScenePaths[i] == path)
				return true;
			i++;
		}

		return false;
	}

	void SpawnTeleportPlayer()
	{
		//Transform playerStartPosition = null;
		//while (playerStartPosition == null)
		//	playerStartPosition = GameObject.FindGameObjectWithTag("BeginPlay").transform;

		Transform beginPlayTransform = GameObject.FindWithTag("BeginPlay").transform;

		if (beginPlayTransform == null)
		{
			Debug.LogWarning("No Begin Play in the Scene ! ");
			return;
		}

		if (PlayerController.Instance == null)
		{
			// If player doesnt exist, we spawn him
			Instantiate(userInterfacePrefab, playerParent);
			Instantiate(playerPrefab, beginPlayTransform.position, beginPlayTransform.rotation, playerParent);
			Instantiate(cameraPrefab, beginPlayTransform.position, Quaternion.identity, playerParent);
		}
		else
		{
			// Otherwise we teleport him
			PlayerController.Instance.Teleport(beginPlayTransform.position, beginPlayTransform.eulerAngles);
		}
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

	public void ReloadLevel()
	{
		if (!_loadingLevel)
			StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().path));
	}

	IEnumerator LoadLevelCoroutine(string scenePath)
	{
		_loadingLevel = true;

		// Check if the new level is a menu or not
		bool isMenu = IsMenu(scenePath);

		// Load loading screen scene
		SceneManager.LoadSceneAsync(loadingScreenScenePath, LoadSceneMode.Additive);
		Scene loadingScreenScene = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);

		// Wait until the loading screen scene to be loaded
		while (LoadingScreen.Instance == null)
			yield return null;

		// Block player inputs
		if (InputsManager.Instance != null && !isMenu)
			InputsManager.Instance.EnableInput(false);

		// Start fade in
		LoadingScreen.Instance.SetTiming(fadeInDuration, fadeOutDuration);
		LoadingScreen.Instance.FadeIn();

		// Get current scene
		Scene oldLevel = SceneManager.GetActiveScene();

		// Destroy all existing projectiles
		for (int i = projectileParent.transform.childCount - 1; i >= 0; i--)
			Destroy(projectileParent.transform.GetChild(i).gameObject);

		//Debug.Log("Scene : " + loadingScreenScene.path);
		//Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);

		// Wait for the fade in to finish
		while (LoadingScreen.Instance.IsFadingIn)
			yield return null;

		// Unload previous level
		SceneManager.UnloadSceneAsync(oldLevel);
		yield return new WaitForSeconds(0.1f);

		// Start loading the new level
		AsyncOperation newLevelAO = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
		newLevelAO.allowSceneActivation = false;


		// Loading in progress
		while (newLevelAO.progress < 0.9f && !newLevelAO.isDone)
		{
			// Loading Screen progress here
			//Debug.Log("Progress : " + newLevelAO.progress);

			//LoadingScreen.Instance.SetProgressBarValue(newLevelAO.progress / 0.9f);
			yield return null;
		}
		
		// Activate new level
		newLevelAO.allowSceneActivation = true;
		yield return new WaitForSeconds(0.1f);

		// Set new level as active level
		Scene newLevel = SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
		SceneManager.SetActiveScene(newLevel);
		//Debug.Log("New Level : " + newLevel.path);

		// Spawn/Teleport player, reset his dash charges and fully heal him, if the scene isn't a menu
		if (!isMenu)
		{
			SpawnTeleportPlayer();
			yield return null;
			PlayerController.Instance.ResetDashCharges();
			PlayerHealth.Instance.Heal();
		}
		else // Otherwise we destroy the player object
		{
			DestroyPlayer();
			yield return null;
		}

		// Wait for the load screen to do its things uh
		yield return new WaitForSeconds(loadingScreenDuration);

		// Start fade out
		LoadingScreen.Instance.FadeOut();

		// Wait for the fade out to finish
		while (LoadingScreen.Instance.IsFadingOut)
			yield return null;

		// Unload loading screen level
		SceneManager.UnloadSceneAsync(loadingScreenScene);

		// Wait a bit and activate player inputs back
		yield return new WaitForSeconds(0.2f);
		if (!isMenu)
			InputsManager.Instance.EnableInput(true);

		//Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);


		//Debug.Log("Nb Scene : " + SceneManager.loadedSceneCount);
		//for (int i = 0; i < SceneManager.loadedSceneCount; i++)
		//{
		//	Debug.Log($"Scene ({i}) : {SceneManager.GetSceneAt(i).path}");
		//}

		_loadingLevel = false;
	}
}
