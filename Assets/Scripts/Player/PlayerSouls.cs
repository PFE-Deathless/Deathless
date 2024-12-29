using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
	int souls;

	public void AddSouls(int amount)
	{
		souls += amount;
		Debug.Log("Souls : " + souls);
	}
}
