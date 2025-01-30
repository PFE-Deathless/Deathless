using UnityEngine;

public class Teleport : MonoBehaviour
{
	public Transform tpA;
	public Transform tpB;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			PlayerController.Instance.Teleport(tpA);
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			PlayerController.Instance.Teleport(tpB);
		}
	}
}
