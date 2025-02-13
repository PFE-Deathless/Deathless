using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
	[HideInInspector] public static PlayerSouls Instance { get; private set; }

	public int souls;
	int tempSouls;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			souls++;
			SoulsDisplay.Instance.UpdateSouls(souls);
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			souls--;
			SoulsDisplay.Instance.UpdateSouls(souls);
		}
	}

	public void AddSouls(int amount)
	{
		tempSouls += amount;
		//Debug.Log("Temp Souls : " + tempSouls);
	}

	public void ValidateSouls()
	{
		souls += tempSouls;
		tempSouls = 0;
		//Debug.Log("Temp Souls : " + tempSouls);
		//Debug.Log("Souls : " + souls);
		SoulsDisplay.Instance.UpdateSouls(souls);
	}
}
