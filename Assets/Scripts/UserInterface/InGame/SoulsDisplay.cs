using UnityEngine;
using TMPro;

public class SoulsDisplay : MonoBehaviour
{
    [HideInInspector] public static SoulsDisplay Instance { get; private set; }

    public TextMeshProUGUI soulsText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateSouls(int value)
	{
		soulsText.text = value.ToString();
	}
}
