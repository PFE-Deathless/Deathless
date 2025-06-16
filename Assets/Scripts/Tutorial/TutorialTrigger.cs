using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
	[Header("Technical")]
	[SerializeField] TextMeshProUGUI textTMP;
	[SerializeField] GameObject popUpUI;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3 || other.gameObject.layer == 6) // Player or player dashing
		{
			InputsManager.Instance.SetMap(Map.Menu);
			popUpUI.SetActive(true);
			TutorialManager.instance.SetTrigger(this);
			Time.timeScale = 0f;
		}
	}
}
