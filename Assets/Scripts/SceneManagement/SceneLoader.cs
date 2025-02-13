using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField] string sceneName;

	AsyncOperation _sceneLoading;
	bool _sceneLoaded;

	private void Update()
	{
		if (_sceneLoaded)
		{
			_sceneLoaded = false;
			_sceneLoading.allowSceneActivation = true;
			GameManager.Instance.activeScene = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			Debug.Log("IN !");
			if (sceneName == GameManager.Instance.HubScene)
			{
				Debug.Log("Go to Hub !");
				GameManager.Instance.GoToHub();

			}
			else
			{
				StartCoroutine(LoadSceneAsync(sceneName));
			}
		}
	}

	IEnumerator LoadSceneAsync(string scene)
	{
		if (scene != string.Empty)
		{
			_sceneLoading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			_sceneLoading.allowSceneActivation = false;
		}

		while (!_sceneLoading.isDone)
		{
			if (_sceneLoading.progress >= 0.9f)
			{
				Debug.Log("Done !");
				_sceneLoaded = true;
				break;
			}
			yield return null;
		}
	}
}
