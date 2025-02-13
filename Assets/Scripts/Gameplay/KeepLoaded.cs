using UnityEngine;

public class KeepLoaded : MonoBehaviour
{
	[HideInInspector] public KeepLoaded Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
}
