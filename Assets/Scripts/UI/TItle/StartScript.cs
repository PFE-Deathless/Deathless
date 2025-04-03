using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
	[SerializeField] string levelPath;
	[SerializeField] float fadeDuration = 0.2f;

	public GameObject canvaActive;
	public GameObject canvaDeactivate;

	public void LoadCanva()
	{
		canvaActive.SetActive(true);
		canvaDeactivate.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadLevel(string path)
	{
		GameManager.Instance.LoadLevel(path);
	}
}
