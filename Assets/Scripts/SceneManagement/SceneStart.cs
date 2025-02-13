using UnityEngine;

public class SceneStart : MonoBehaviour
{
	[SerializeField, Tooltip("Transform that the player will be teleported to when entering the scene")] Transform teleportTransform;

	void Awake()
	{
		Debug.Log("Scene loaded !");
		PlayerController.Instance.Teleport(teleportTransform.position, teleportTransform.eulerAngles);
	}
}
