using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Controller type")]
	[SerializeField] HitType.Controller controller;
	
	[Header("Transition")]
	[SerializeField] private string hubScenePath;
	[SerializeField] private string gameScenePath;
	[SerializeField] private float blackScreenDuration = 1f;

	[Header("Projectiles")]
	[SerializeField] private Transform projectileParent;

	[Header("Debug")]
	[SerializeField] private bool disableFadeTransition;

	public string HubScenePath => hubScenePath;

	public Transform ProjectileParent => projectileParent;

	private Scene _currentLevelScene;
	private bool _isLoading;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		HitType.SetController(controller);
		EnemyBarks.InitBarks();

		StartCoroutine(EnsurePersistentScenesLoaded());
	}

	private void Update()
	{
		if (InputsManager.Instance.reloadScene)
		{
			InputsManager.Instance.reloadScene = false;
			ReloadCurrentLevel();
		}
	}

	private IEnumerator EnsurePersistentScenesLoaded()
	{
		// Only load Hub and Game scene if they aren't already loaded
		if (!IsSceneLoaded(hubScenePath))
			yield return SceneManager.LoadSceneAsync(hubScenePath, LoadSceneMode.Additive);

		if (!IsSceneLoaded(gameScenePath))
			yield return SceneManager.LoadSceneAsync(gameScenePath, LoadSceneMode.Additive);
	}

	public void LoadLevel(string levelPath)
	{
		if (_isLoading || string.IsNullOrEmpty(levelPath))
			return;

		StartCoroutine(LoadLevelCoroutine(levelPath));
	}

	private IEnumerator LoadLevelCoroutine(string levelPath)
	{
		_isLoading = true;

		if (!disableFadeTransition)
		{
			// Block player inputs
			InputsManager.Instance.canInput = false;

			// Fade the screen black
			FadeScreen.Instance.StartFadeIn();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;
		}

		// Load new level additively
		AsyncOperation newLevelLoad = SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Additive);
		newLevelLoad.allowSceneActivation = false;

		while (!newLevelLoad.isDone)
		{
			if (newLevelLoad.progress >= 0.9f)
				break;

			yield return null;
		}

		// Unload the previous level (if any)
		if (_currentLevelScene.IsValid() && _currentLevelScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(_currentLevelScene);
		}

		// Activate the new level
		newLevelLoad.allowSceneActivation = true;

		// Wait until Unity actually registers the scene as loaded
		yield return new WaitUntil(() => SceneManager.GetSceneByPath(levelPath).isLoaded);

		// Store the newly loaded level
		_currentLevelScene = SceneManager.GetSceneByPath(levelPath);
		SceneManager.SetActiveScene(_currentLevelScene);

		if (!disableFadeTransition)
		{
			// Wait a bit in black screen
			yield return new WaitForSeconds(blackScreenDuration);

			// Fade the screen from black
			FadeScreen.Instance.StartFadeOut();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;

			// Re enable player inputs
			InputsManager.Instance.canInput = true;
		}

		_isLoading = false;
	}

	public void ReloadCurrentLevel()
	{
		if (_isLoading || !_currentLevelScene.IsValid() || !_currentLevelScene.isLoaded)
			return;

		StartCoroutine(ReloadLevelCoroutine());
	}

	private IEnumerator ReloadLevelCoroutine()
	{
		_isLoading = true;

		if (!disableFadeTransition)
		{
			// Block player inputs
			InputsManager.Instance.canInput = false;

			// Fade the screen black
			FadeScreen.Instance.StartFadeIn();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;
		}

		string levelPath = _currentLevelScene.path;

		// Unload the current level first
		yield return SceneManager.UnloadSceneAsync(_currentLevelScene);

		// Load the new instance
		AsyncOperation newLevelLoad = SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Additive);
		yield return newLevelLoad; // Wait for loading to finish

		// Set the new level as active
		_currentLevelScene = SceneManager.GetSceneByPath(levelPath);
		SceneManager.SetActiveScene(_currentLevelScene);

		if (!disableFadeTransition)
		{
			// Wait a bit in black screen
			yield return new WaitForSeconds(blackScreenDuration);

			// Fade the screen black
			FadeScreen.Instance.StartFadeOut();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;

			// Re enable player inputs
			InputsManager.Instance.canInput = true;
		}

		_isLoading = false;
	}

	public void ReturnToHub()
	{
		if (_isLoading)
			return;

		StartCoroutine(ReturnToHubCoroutine());
	}

	private IEnumerator ReturnToHubCoroutine()
	{
		if (!disableFadeTransition)
		{
			// Block player inputs
			InputsManager.Instance.canInput = false;

			// Fade the screen black
			FadeScreen.Instance.StartFadeIn();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;
		}

		Transform t = SceneHubStart.Instance.GetTransformFromPath(_currentLevelScene.path);
		PlayerController.Instance.Teleport(t.position, t.eulerAngles);

		yield return StartCoroutine(UnloadCurrentLevel());

		if (!disableFadeTransition)
		{
			// Wait a bit in black screen
			yield return new WaitForSeconds(blackScreenDuration);

			// Fade the screen black
			FadeScreen.Instance.StartFadeOut();

			// Wait for fade to be done
			while (!FadeScreen.Instance.FadingDone)
				yield return null;

			// Re enable player inputs
			InputsManager.Instance.canInput = true;
		}
	}

	private IEnumerator UnloadCurrentLevel()
	{
		_isLoading = true;

		if (_currentLevelScene.IsValid() && _currentLevelScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(_currentLevelScene);
		}

		_currentLevelScene = default; // Reset level tracking
		_isLoading = false;
	}

	private bool IsSceneLoaded(string scenePath)
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.path == scenePath)
				return true;
		}
		return false;
	}
}
