using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour
{
	public static DashDisplay Instance { get; private set; }

	[SerializeField] float blinkTime = 0.3f;
	[SerializeField] Image[] dashImages;

	Color _originalColor;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		_originalColor = dashImages[0].color;
	}

	public void SetDashCooldown(int remainingDashes, float percentage)
	{
		for (int i = 0; i < dashImages.Length; i++)
			dashImages[i].fillAmount = i < remainingDashes ? 1f : 0f;

		if (remainingDashes < dashImages.Length)
			dashImages[remainingDashes].fillAmount = percentage;
	}

	public void BlinkColor(int dashCharges)
	{
		StartCoroutine(BlinkCoroutine(dashCharges));
	}

	IEnumerator BlinkCoroutine(int index)
	{
		dashImages[index].color = Color.red;
		yield return new WaitForSeconds(blinkTime);
		dashImages[index].color =  _originalColor;
	}
}
