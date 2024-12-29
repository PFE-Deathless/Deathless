using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
	int souls;

	[HideInInspector] public GameManager gameManager;

	public void AddSouls(int amount)
	{
		souls += amount;
		Debug.Log("Souls : " + souls);
	}
}
