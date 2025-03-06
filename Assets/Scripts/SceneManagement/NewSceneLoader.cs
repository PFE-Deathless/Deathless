using UnityEngine;

public class NewSceneLoader : MonoBehaviour
{
	[SerializeField] string scenePath;
	[SerializeField] LayerMask playerLayer = (1 << 3) | (1 << 6);

    private void OnTriggerEnter(Collider other)
	{
		if ((playerLayer & (1 << other.gameObject.layer)) != 0)
		{
			NewGameManager.Instance.LoadLevel(scenePath);
		}
	}
}
