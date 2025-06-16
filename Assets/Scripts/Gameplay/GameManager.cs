using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Objects to spawn on level start")]
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject userInterfacePrefab;
	[SerializeField] GameObject cameraPrefab;
	[SerializeField] GameObject audioManagerPrefab;
	[SerializeField] GameObject tombInterfacePrefab;
	[SerializeField, Tooltip("Transform the player objects will be attached to")] Transform playerParent;

	[Header("Save/Load player data")]
	[SerializeField] string saveFileName = "data.sav";

	[Header("Tomb System")]
	[SerializeField] string tombTSVFileName = "Texte_Tombes";

	[Header("Scene Transition")]
	[SerializeField] LoadingScreen loadingScreen;
	[SerializeField] float fadeInDuration = 0.5f;
	[SerializeField] float fadeOutDuration = 0.5f;
	[SerializeField] float loadingScreenDuration = 1f;

	[Header("Menu Scene")]
	[SerializeField] string mainMenuScenePath;
	[SerializeField] string[] menuScenePaths;

	[Header("Projectiles")]
	[SerializeField, Tooltip("Transform the projectiles will be attached to")] Transform projectileParent;

	// Public Properties
	public PlayerData playerData;

	// ### Private properties ###
	// Level Loading
	bool _loadingLevel = false;

	// Save/Load system
	string _savePath;
	private bool _doesSaveExist = false;

	// Tomb system
	private TombData[] _tombData;
	private GameObject _tombInterfaceObject;
	private TombInterface _tombInterface;
	private bool _isShowingTomb = false;
	private Transform _tombTransform;

	// Key System
	int _keys;

	// Debug screen
	string _debugText = "";

	// Public attributes
	public bool LevelIsLoading => _loadingLevel;
	public Transform ProjectileParent => projectileParent;
	public bool IsShowingTomb => _isShowingTomb;
	public bool DoesSaveExist => _doesSaveExist;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);

		Debug.developerConsoleEnabled = true;
		Debug.developerConsoleVisible = true;
	}

	private void Start()
	{
#if !UNITY_EDITOR
		Cursor.visible = false;
#endif
		Debug.developerConsoleEnabled = true;
		Debug.developerConsoleVisible = true;

		_savePath = Path.Combine(Application.persistentDataPath, saveFileName);

		// Set loading screen timings for fadings
		loadingScreen.SetTiming(fadeInDuration, fadeOutDuration);

		LoadData();

		LoadTombText();

		if (!IsMenu(SceneManager.GetActiveScene().path))
		{
			SpawnTeleportPlayer();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			SaveData();
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			Debug.LogError("X !");
			Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
		}

		if (Input.GetKeyDown(KeyCode.U))
		{
			LoadData();
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			ResetData();
		}

		if (_isShowingTomb && _tombTransform != null && InputsManager.Instance.cancel)
		{
			HideTombData();
			InputsManager.Instance.cancel = false;
		}

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

	#region DUNGEON_SYSTEM

	public bool IsUnlocked(Dungeon dungeon)
	{
		switch (dungeon)
		{
			case Dungeon.None:
				return true;
			case Dungeon.Tutorial:
				return playerData.tutorial;
			case Dungeon.Dungeon1:
				return playerData.dungeon1;
			case Dungeon.Dungeon2:
				return playerData.dungeon2;
			case Dungeon.Dungeon3:
				return playerData.dungeon3;
			case Dungeon.Dungeon4:
				return playerData.dungeon4;
			case Dungeon.Dungeon5:
				return playerData.dungeon5;
			default:
				return false;
		}
	}

	public void UnlockDungeon(Dungeon dungeon)
	{
		switch (dungeon)
		{
			case Dungeon.None:
				return;
			case Dungeon.Tutorial:
				playerData.tutorial = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon1:
				playerData.dungeon1 = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon2:
				playerData.dungeon2 = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon3:
				playerData.dungeon3 = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon4:
				playerData.dungeon4 = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon5:
				playerData.dungeon5 = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			default:
				return;
		}
	}

	public bool HasDungeonSoul(Dungeon dungeon)
	{
		switch (dungeon)
		{
			case Dungeon.None:
				return true;
			case Dungeon.Tutorial:
				return playerData.tutorialSoul;
			case Dungeon.Dungeon1:
				return playerData.dungeon1Soul;
			case Dungeon.Dungeon2:
				return playerData.dungeon2Soul;
			case Dungeon.Dungeon3:
				return playerData.dungeon3Soul;
			case Dungeon.Dungeon4:
				return playerData.dungeon4Soul;
			case Dungeon.Dungeon5:
				return playerData.dungeon5Soul;
			default:
				return false;
		}
	}

	public void UnlockDungeonSoul(Dungeon dungeon)
	{
		switch (dungeon)
		{
			case Dungeon.None:
				return;
			case Dungeon.Tutorial:
				playerData.tutorialSoul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon1:
				playerData.dungeon1Soul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon2:
				playerData.dungeon2Soul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon3:
				playerData.dungeon3Soul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon4:
				playerData.dungeon4Soul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			case Dungeon.Dungeon5:
				playerData.dungeon5Soul = true;
				SoulsDisplay.Instance.UpdateSouls();
				SaveData();
				return;
			default:
				return;
		}
	}

	#endregion

	#region KEYS

	public void AddKey()
	{
		_keys++;
		KeyDisplay.Instance.AddKey(_keys);
	}

	public bool UseKey()
	{
		if (_keys >= 1)
		{
			_keys--;
			KeyDisplay.Instance.RemoveKey(_keys);
			return true;
		}
		return false;
	}

	#endregion

	#region TOMB_SYSTEM

	void LoadTombText()
	{
		string path = Path.Combine("Tomb", tombTSVFileName);

		TextAsset temp = Resources.Load<TextAsset>(path);
		string[] tempArr = temp.text.Split("\r\n");

		_tombData = new TombData[tempArr.Length - 1];

		for (int i = 0; i < tempArr.Length - 1; i++)
			_tombData[i] = new TombData(tempArr[i + 1]);
	}

	public TombData GetTombData(uint id)
	{
		if (id > _tombData.Length || id == 0)
			return new(null);
		return _tombData[id - 1];
	}

	public void HideTombData()
	{
		_isShowingTomb = false;
		_tombTransform = null;
		InputsManager.Instance.SetMap(Map.Gameplay);

		_tombInterfaceObject.SetActive(false);
	}

	public void ShowTombData(TombData data, Transform tomb)
	{
		_isShowingTomb = true;
		InputsManager.Instance.SetMap(Map.Menu);

		_tombInterface.ShowData(data, IsUnlocked(data.dungeon));
		_tombTransform = tomb;

		_tombInterfaceObject.SetActive(_isShowingTomb);
	}

	#endregion

	#region SAVE_PLAYER_DATA

	public void SaveData()
	{
		string json = JsonUtility.ToJson(playerData, true);
		File.WriteAllText(_savePath, json);
		//Debug.Log("Game Saved at : " + _savePath + " !\n" + json);
	}

	public void LoadData()
	{
		if (!File.Exists(_savePath))
		{
			playerData = new();
			_doesSaveExist = false;
			return;
		}

		string json = File.ReadAllText(_savePath);
		playerData = JsonUtility.FromJson<PlayerData>(json);
		_doesSaveExist = true;
		//Debug.Log("Game Loaded from : " + _savePath + " !\n" + json);
	}

	public void ResetData()
	{
		//Debug.Log("Data reset !");
		playerData = new();
		SaveData();
	}

	#endregion

	#region LEVEL_MANAGER

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
		GameObject beginPlayObj;
		do
		{
			beginPlayObj = GameObject.FindWithTag("BeginPlay");
			//LogText("Searching for Begin Play...");
		} while (beginPlayObj == null);

		//LogText("Begin Play found !");
		Transform beginPlayTransform = beginPlayObj.transform;

		if (beginPlayTransform == null)
		{
			//Debug.LogWarning("No Begin Play in the Scene ! ");
			//LogText("No Begin Play in the Scene ! ");
			return;
		}

		if (PlayerController.Instance == null)
		{
			// If player doesnt exist, we spawn him
			Instantiate(userInterfacePrefab, playerParent);
			Instantiate(playerPrefab, beginPlayTransform.position, beginPlayTransform.rotation, playerParent);
			Instantiate(cameraPrefab, beginPlayTransform.position, Quaternion.identity, playerParent);
			Instantiate(audioManagerPrefab, playerParent);
			_tombInterfaceObject = Instantiate(tombInterfacePrefab, playerParent);
			_tombInterfaceObject.SetActive(false);
			_tombInterface = _tombInterfaceObject.GetComponent<TombInterface>();
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
		{
			LoadData();
			if (PlayerInteract.Instance != null)
				PlayerInteract.Instance.ClearInteract();
			StartCoroutine(LoadLevelCoroutine(scenePath));
		}
	}

	public void ReloadLevel()
	{
		if (!_loadingLevel)
		{
			LoadData();
			if (PlayerInteract.Instance != null)
				PlayerInteract.Instance.ClearInteract();
			StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().path));
		}
	}


	IEnumerator LoadLevelCoroutine(string scenePath)
	{
		_loadingLevel = true;

		// Check if the new level is a menu or not
		bool isMenu = IsMenu(scenePath);

		// Block player inputs
		if (InputsManager.Instance != null && !isMenu)
			InputsManager.Instance.EnableInput(false);

		// Start loading screen fade in, and wait for it to finish
		loadingScreen.FadeIn();
		while (loadingScreen.IsFadingIn)
		{
			//LogText("Fading in...");
			yield return null;
		}

		// Load new scene
		AsyncOperation newLevelAO = SceneManager.LoadSceneAsync(scenePath);
		newLevelAO.allowSceneActivation = false;

		// Loading in progress (wait for it to finish)
		while (newLevelAO.progress < 0.9f && !newLevelAO.isDone)
		{
			// Loading Screen progress here
			//LogText("Progress : " + newLevelAO.progress);

			loadingScreen.SetProgressBarValue(newLevelAO.progress / 0.9f);
			yield return null;
		}

		// Skip a frame (just to be sure)
		yield return null;

		// Activate the new scene afterwards
		newLevelAO.allowSceneActivation = true;

		// Wait for the active scene to be the newly loaded scene
		while (SceneManager.GetActiveScene().path != scenePath)
		{
			//LogText("Loading new scene...");
			yield return null;
		}

		//LogText("Active Scene path : " + SceneManager.GetActiveScene().path);


		// ### Scene changes ###

		// Reset keys number
		_keys = 0;
		if (KeyDisplay.Instance != null)
			KeyDisplay.Instance.SetKeyNumber(_keys);

		// Destroy all existing projectiles
		for (int i = projectileParent.transform.childCount - 1; i >= 0; i--)
			Destroy(projectileParent.transform.GetChild(i).gameObject);

		// Skip a frame (just to be sure)
		yield return null;

		// Spawn/Teleport player, reset his dash charges and fully heal him, if the scene isn't a menu
		if (!isMenu)
		{
			SpawnTeleportPlayer();
			yield return null;
			PlayerController.Instance.ResetDashCharges();
			PlayerHealth.Instance.HealFull();
			SoulsDisplay.Instance.UpdateSouls();
		}
		else // Otherwise we destroy the player object
		{
			DestroyPlayer();
			yield return null;
		}

		// Wait a bit for the loading screen to finish (so the player is correclty teleported)
		yield return new WaitForSecondsRealtime(loadingScreenDuration);

		// #####################


		// Start loading screen fade out, and wait for it to finish
		loadingScreen.FadeOut();
		while (loadingScreen.IsFadingOut)
		{
			//LogText("Fading out...");
			yield return null;
		}

		// Wait a bit and activate player inputs back
		yield return new WaitForSecondsRealtime(0.2f);
		if (!isMenu)
			InputsManager.Instance.EnableInput(true);

		_loadingLevel = false;
	}

	#endregion

	#region DEBUG

	public void LogText(string log)
	{
		if (Debug.isDebugBuild)
		{
			_debugText += $"\n [{DateTime.Now}] : {log}";
			File.WriteAllText(Path.Combine(Application.persistentDataPath, "debug.log"), _debugText);
			Debug.Log(log);
		}
	}

	#endregion
}

public struct TombData
{
	public Dungeon dungeon;
	public string name;
	public string date;
	public string epitaph;
	public string memory;

	public TombData(string data)
	{
		if (data == null)
		{
			dungeon = Dungeon.None;
			name = "DEFAULT_NAME";
			date = "DEFAULT_DATE";
			epitaph = "DEFAULT_EPITAPH";
			memory = "DEFAULT_MEMORY";
		}
		else
		{
			string[] temp = data.Split("\t");
			dungeon = temp[1] switch
			{
				"Tutorial" => Dungeon.Tutorial,
				"Donjon_1" => Dungeon.Dungeon1,
				"Donjon_2" => Dungeon.Dungeon2,
				"Donjon_3" => Dungeon.Dungeon3,
				"Donjon_4" => Dungeon.Dungeon4,
				"Donjon_5" => Dungeon.Dungeon5,
				_ => Dungeon.None,
			};
			name = temp[2];
			date = temp[3];
			epitaph = temp[4];
			memory = temp[5];
		}
	}
}

public enum Dungeon
{
	None,
	Tutorial,
	Dungeon1,
	Dungeon2,
	Dungeon3,
	Dungeon4,
	Dungeon5,
}

public class PlayerData
{
	public bool dungeon1 = false;
	public bool dungeon2 = false;
	public bool dungeon3 = false;
	public bool dungeon4 = false;
	public bool dungeon5 = false;
	public bool tutorial = false;

	public bool dungeon1Soul = false;
	public bool dungeon2Soul = false;
	public bool dungeon3Soul = false;
	public bool dungeon4Soul = false;
	public bool dungeon5Soul = false;
	public bool tutorialSoul = false;
}