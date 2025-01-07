using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
	public int souls;

	[HideInInspector] public GameManager gameManager;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			souls++;
			gameManager.soulsDisplay.UpdateSouls(souls);
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			souls--;
			gameManager.soulsDisplay.UpdateSouls(souls);
		}
	}

	public void AddSouls(int amount)
	{
		souls += amount;
		gameManager.soulsDisplay.UpdateSouls(souls);
		Debug.Log("Souls : " + souls);
	}
}
