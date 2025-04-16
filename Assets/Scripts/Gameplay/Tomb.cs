using UnityEngine;

public class Tomb : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField, Tooltip("ID of the text to show when interacting with the tomb")] uint ID = 0;
	[SerializeField, Tooltip("Dungeon that will unlock the memory of this tomb")] Dungeon dungeonToUnlock;

	[Header("Technical")]
	[SerializeField] GameObject tombParticle;

	bool _isUnlocked = false;

	void Start()
	{
		Invoke(nameof(Init), 0.1f);
	}

	void Init()
	{
		_isUnlocked = GameManager.Instance.IsUnlocked(dungeonToUnlock);
		tombParticle.SetActive(_isUnlocked);
	}

	public void Interact()
	{
		Debug.Log("Epitaph : \n" + GameManager.Instance.GetTombEpitah(ID));
		if (_isUnlocked)
			Debug.Log("Memory : \n" + GameManager.Instance.GetTombMemory(ID));
		else
			Debug.Log("Memory isn't unlocked !");
	}
}
