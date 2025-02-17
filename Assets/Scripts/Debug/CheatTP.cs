using UnityEngine;

public class CheatTP : MonoBehaviour
{
	public Transform[] listCheckpoint = new Transform[9];

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad0))
			PlayerController.Instance.Teleport(listCheckpoint[0].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad1))
			PlayerController.Instance.Teleport(listCheckpoint[1].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad2))
			PlayerController.Instance.Teleport(listCheckpoint[2].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad3))
			PlayerController.Instance.Teleport(listCheckpoint[3].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad4))
			PlayerController.Instance.Teleport(listCheckpoint[4].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad5))
			PlayerController.Instance.Teleport(listCheckpoint[5].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad6))
			PlayerController.Instance.Teleport(listCheckpoint[6].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad7))
			PlayerController.Instance.Teleport(listCheckpoint[7].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad8))
			PlayerController.Instance.Teleport(listCheckpoint[8].position, Vector3.zero);
	}
}
