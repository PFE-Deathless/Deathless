using UnityEngine;

public class StartScript : MonoBehaviour
{
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
