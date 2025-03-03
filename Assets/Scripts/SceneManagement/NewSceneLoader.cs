using UnityEngine;

public class NewSceneLoader : MonoBehaviour
{
	[SerializeField] string scenePath;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			NewGameManager.Instance.LoadLevel(scenePath);
		}
	}
}
