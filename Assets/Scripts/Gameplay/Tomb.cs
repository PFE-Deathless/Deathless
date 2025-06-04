using UnityEngine;

public class Tomb : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField, Tooltip("ID of the text to show when interacting with the tomb")] uint ID = 0;
	[SerializeField] InteractableType interactableType;

	[Header("Technical")]
	[SerializeField] GameObject tombParticle;

	bool _isUnlocked = false;
	TombData _data;

	void Start()
	{
		tombParticle.SetActive(false);
		Invoke(nameof(Init), 0.1f);
	}

	void Init()
	{
		_data = GameManager.Instance.GetTombData(ID);
		CheckUnlock();
	}

	public void CheckUnlock()
	{
		_isUnlocked = GameManager.Instance.IsUnlocked(_data.dungeon);
		tombParticle.SetActive(_isUnlocked);
	}

	public void Interact(InteractableType type)
	{
		if (interactableType != type && interactableType != InteractableType.Both)
			return;

		if (!GameManager.Instance.IsShowingTomb)
			GameManager.Instance.ShowTombData(_data, transform);
		else
			GameManager.Instance.HideTombData();

		//	string text = "";
		//text += $"Name : {_data.name}\n";
		//text += $"Date : {_data.date}\n";
		//text += $"Epitaph : {_data.epitaph}\n";
		//if (_isUnlocked)
		//	text += $"Memory : {_data.memory}\n";
		//else
		//	text += $"Memory : {_data.dungeon} hasn't be unlocked yet !\n";

		//Debug.Log(text);
	}
}
