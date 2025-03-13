using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour
{
	public static DashDisplay Instance { get; private set; }

	[SerializeField] Image[] dashImages;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void SetDashCooldown(int remainingDashes, float percentage)
	{
		for (int i = 0; i < dashImages.Length; i++)
			dashImages[i].fillAmount = i < remainingDashes ? 1f : 0f;

		if (remainingDashes < dashImages.Length)
			dashImages[remainingDashes].fillAmount = percentage;
	}
}
