using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	[SerializeField] HitType.Controller controller;
	[SerializeField, Tooltip("Object to keep loaded between scenes")] GameObject[] objectToKeepLoaded;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		//// Keep loaded between scenes
		//foreach (GameObject obj in objectToKeepLoaded)
		//{
		//	if (obj != gameObject)
		//		DontDestroyOnLoad(obj);
		//}
		//DontDestroyOnLoad(gameObject);

		//DontDestroyOnLoad(SceneManager.GetActiveScene());

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
