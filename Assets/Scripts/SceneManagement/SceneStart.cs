using UnityEngine;

public class SceneStart : MonoBehaviour
{
	void Awake()
	{
		PlayerController.Instance.Teleport(transform.position, transform.eulerAngles);
	}
}
