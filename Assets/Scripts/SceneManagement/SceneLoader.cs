using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField, Tooltip("Scene this loader will load (eh)")] string sceneName;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
		{
			if (sceneName == GameManager.Instance.HubScenePath)
				GameManager.Instance.ReturnToHub();
			else
				GameManager.Instance.LoadLevel(sceneName);
		}
	}
}
