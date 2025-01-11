using UnityEngine;
using TMPro;

public class SoulsDisplay : MonoBehaviour
{
	public TextMeshProUGUI soulsText;

	public void UpdateSouls(int value)
	{
		soulsText.text = value.ToString();
	}
}
