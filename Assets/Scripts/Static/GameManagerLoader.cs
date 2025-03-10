#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GameManagerLoader
{
	static GameManagerLoader()
	{
		EditorApplication.playModeStateChanged += LoadGameManager;
	}

	static void LoadGameManager(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.ExitingEditMode)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		if (state == PlayModeStateChange.EnteredPlayMode)
		{
			Object gameManagerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Gameplay/GameManager.prefab", typeof(GameObject));
			PrefabUtility.InstantiatePrefab(gameManagerPrefab);
			//EditorSceneManager.LoadSceneAsync("Assets/Scenes/GameManagerScene.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}
	}
}
#endif