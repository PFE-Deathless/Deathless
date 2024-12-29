using UnityEngine;

public class GameManager : MonoBehaviour
{
	GameObject player;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
	}
}
