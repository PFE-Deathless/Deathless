using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] HitType.Controller controller;

	GameObject player;
	GameObject uiPlayer;

	PlayerHealth playerHealth;
	PlayerSouls playerSouls;

	public InputsManager inputsManager { get; private set; }

	public HealthDisplay healthDisplay { get; private set; }
	public SoulsDisplay soulsDisplay { get; private set; }

	void Awake()
	{
		HitType.SetController(controller);
		EnemyBarks.InitBarks();

		player = GameObject.FindWithTag("Player");
		uiPlayer = GameObject.FindWithTag("UIPlayer");

		playerHealth = player.GetComponent<PlayerHealth>();
		playerSouls = player.GetComponent<PlayerSouls>();

		inputsManager = player.GetComponent<InputsManager>();

		healthDisplay = uiPlayer.GetComponent<HealthDisplay>();
		soulsDisplay = uiPlayer.GetComponent<SoulsDisplay>();

		playerHealth.gameManager = this;
		playerSouls.gameManager = this;
	}

	private void Update()
	{
		if (player != null)
		{
			if (inputsManager.reloadScene)
			{
				inputsManager.reloadScene = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}
