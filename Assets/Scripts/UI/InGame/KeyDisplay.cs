using TMPro;
using UnityEngine;

public class KeyDisplay : MonoBehaviour
{
	public static KeyDisplay Instance { get; private set; }

	[SerializeField] TextMeshProUGUI keyNumberTMP;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void SetKeyNumber(int number)
	{
		keyNumberTMP.text = number.ToString();
	}
}
