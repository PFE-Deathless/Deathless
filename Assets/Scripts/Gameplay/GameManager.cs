using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	[SerializeField] HitType.Controller controller;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		HitType.SetController(controller);
		EnemyBarks.InitBarks();
	}

	private void Update()
	{
		if (InputsManager.Instance.reloadScene)
		{
			InputsManager.Instance.reloadScene = false;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
