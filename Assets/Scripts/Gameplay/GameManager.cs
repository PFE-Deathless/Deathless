using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	GameObject player;
	GameObject uiPlayer;
	PlayerHealth playerHealth;
	PlayerSouls playerSouls;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		uiPlayer = GameObject.FindWithTag("UIPlayer");
		playerHealth = player.GetComponent<PlayerHealth>();
		playerSouls = player.GetComponent<PlayerSouls>();
		playerHealth.gameManager = this;
		playerSouls.gameManager = this;
	}

	private void Update()
	{
		if (player != null)
		{
			if (player.GetComponent<InputsManager>().reloadScene)
			{
				player.GetComponent<InputsManager>().reloadScene = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}
