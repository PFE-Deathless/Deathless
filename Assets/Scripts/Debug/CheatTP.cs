using UnityEngine;

public class CheatTP : MonoBehaviour
{
	public Transform[] listCheckpoint;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad0) && listCheckpoint.Length > 0)
			PlayerController.Instance.Teleport(listCheckpoint[0].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad1) && listCheckpoint.Length > 1)
			PlayerController.Instance.Teleport(listCheckpoint[1].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad2) && listCheckpoint.Length > 2)
			PlayerController.Instance.Teleport(listCheckpoint[2].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad3) && listCheckpoint.Length > 3)
			PlayerController.Instance.Teleport(listCheckpoint[3].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad4) && listCheckpoint.Length > 4)
			PlayerController.Instance.Teleport(listCheckpoint[4].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad5) && listCheckpoint.Length > 5)
			PlayerController.Instance.Teleport(listCheckpoint[5].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad6) && listCheckpoint.Length > 6)
			PlayerController.Instance.Teleport(listCheckpoint[6].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad7) && listCheckpoint.Length > 7)
			PlayerController.Instance.Teleport(listCheckpoint[7].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad8) && listCheckpoint.Length > 8)
			PlayerController.Instance.Teleport(listCheckpoint[8].position, Vector3.zero);
		if (Input.GetKeyDown(KeyCode.Keypad9) && listCheckpoint.Length > 9)
			PlayerController.Instance.Teleport(listCheckpoint[9].position, Vector3.zero);
	}
}
