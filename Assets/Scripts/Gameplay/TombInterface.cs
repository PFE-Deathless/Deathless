using TMPro;
using UnityEngine;

public class TombInterface : MonoBehaviour
{
	[Header("Locked")]
	[SerializeField] GameObject lockedParent;
	[SerializeField] TextMeshProUGUI lockedName;
	[SerializeField] TextMeshProUGUI lockedDate;
	[SerializeField] TextMeshProUGUI lockedEpitaph;
	
	[Header("Unlocked")]
	[SerializeField] GameObject unlockedParent;
	[SerializeField] TextMeshProUGUI unlockedName;
	[SerializeField] TextMeshProUGUI unlockedDate;
	[SerializeField] TextMeshProUGUI unlockedEpitaph;
	[SerializeField] TextMeshProUGUI unlockedMemory;

	public void ShowData(TombData data, bool isUnlocked)
	{
		if (!isUnlocked)
		{
			lockedName.text = data.name;
			lockedDate.text = data.date;
			lockedEpitaph.text = data.epitaph;
		}
		else
		{
			unlockedName.text = data.name;
			unlockedDate.text = data.date;
			unlockedEpitaph.text = data.epitaph;
			unlockedMemory.text = data.memory;
		}

		lockedParent.SetActive(!isUnlocked);
		unlockedParent.SetActive(isUnlocked);
	}
}
