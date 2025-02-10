
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
	public string sceneName;
	public Vector3 position;
	public Vector3 rotation;

	AsyncOperation sceneLoading;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(LoadSceneAsync(sceneName));
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			TeleportToNewScene();
		}
	}

	public void TeleportToNewScene()
	{
		sceneLoading.allowSceneActivation = true;
		PlayerController.Instance.Teleport(position, rotation);
		Destroy(gameObject);
	}

	IEnumerator LoadSceneAsync(string scene)
	{
		if (scene != string.Empty)
		{
			sceneLoading = SceneManager.LoadSceneAsync(scene);
			sceneLoading.allowSceneActivation = false;
		}

		while (!sceneLoading.isDone)
		{
			if (sceneLoading.progress >= 0.9f)
			{
				Debug.Log("Done !");
				break;
			}
			yield return null;
		}
	}

}
