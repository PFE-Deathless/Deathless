using UnityEngine;
using UnityEngine.UI;

public class HitDisplay : MonoBehaviour
{
	public static HitDisplay Instance { get; private set; }

	[SerializeField] Image[] hitVigns;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void SetVignPercentage(float percentage)
	{
		//foreach (Image vign in hitVigns)
		//	vign.fillAmount = percentage;
	}
}
