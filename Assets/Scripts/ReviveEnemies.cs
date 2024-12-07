using UnityEngine;

public class ReviveEnemies : MonoBehaviour
{
	public GameObject[] enemies;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.K))
		{
			foreach (GameObject go in enemies)
			{
				go.SetActive(true);
				//go.GetComponent<Enemy>().health = 3;
				go.GetComponent<Enemy>().UpdateHealthInterface();
			}
		}
	}
}
