using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField] string sceneName;
	[SerializeField] Vector3 position;
	[SerializeField] Vector3 rotation;

	AsyncOperation _sceneLoading;
	bool _sceneLoaded;

	private void Update()
	{
		if (_sceneLoaded)
		{
			_sceneLoaded = false;
			TeleportToNewScene();
			Destroy(gameObject);
		}
	}

	public void TeleportToNewScene()
	{
		_sceneLoading.allowSceneActivation = true;
		PlayerController.Instance.Teleport(position, rotation);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			Debug.Log("IN !");
			StartCoroutine(LoadSceneAsync(sceneName));
		}
	}

	IEnumerator LoadSceneAsync(string scene)
	{
		DontDestroyOnLoad(gameObject);

		if (scene != string.Empty)
		{
			_sceneLoading = SceneManager.LoadSceneAsync(scene);
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
