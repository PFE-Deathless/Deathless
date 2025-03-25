using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] string scenePath;
	[SerializeField] bool removeGreenArea = true;

	[Header("Technical")]
	[SerializeField] LayerMask playerLayer = (1 << 3) | (1 << 6);
	[SerializeField] GameObject mesh;

    private void Start()
    {
		if (removeGreenArea)
			mesh.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
	{
		if ((playerLayer & (1 << other.gameObject.layer)) != 0)
		{
			GameManager.Instance.LoadLevel(scenePath);
		}
	}
}
