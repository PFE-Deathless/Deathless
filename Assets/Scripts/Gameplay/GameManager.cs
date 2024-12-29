using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	GameObject player;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
	}

	private void Update()
	{
		if (player != null && player.GetComponent<InputsManager>().reloadScene)
		{
			player.GetComponent<InputsManager>().reloadScene = false;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
