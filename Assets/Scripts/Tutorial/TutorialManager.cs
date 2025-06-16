using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public static TutorialManager instance;

	[SerializeField] Door dummyDoor;
	[SerializeField] Dummy dummy;
	[SerializeField] Enemy[] enemies;
	[SerializeField] Door enemiesDoor;

	bool _allEnemiesDied = false;

	TutorialTrigger activeTrigger;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Update()
	{
		if (InputsManager.Instance != null && InputsManager.Instance.cancel)
		{
			InputsManager.Instance.cancel = false;
			if (activeTrigger != null)
			{
				Time.timeScale = 1f;
				Destroy(activeTrigger.gameObject);
				activeTrigger = null;
				InputsManager.Instance.SetMap(Map.Gameplay);
			}
		}

		if (dummy.GetComponent<Dummy>().Died == true)
			if (dummyDoor.TryGetComponent(out IActivable activable))
				activable.Activate();

		_allEnemiesDied = true;
		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i] != null)
				_allEnemiesDied = false;
		}

		if (_allEnemiesDied)
			if (enemiesDoor.TryGetComponent(out IActivable activable))
				activable.Activate();


	}

	public void SetTrigger(TutorialTrigger trigger)
	{
		activeTrigger = trigger;
	}
}
