using UnityEngine;

public class Key : MonoBehaviour
{
	[SerializeField] Collider triggerCollider;

	public void DropKey()
	{
		transform.parent = null;
		Vector3 newPos = transform.position;
		newPos.y = 0.2f;
		transform.position = newPos;
		triggerCollider.enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (GameManager.Instance.LevelIsLoading)
			return;

		if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
		{
			GameManager.Instance.AddKey();
			Destroy(gameObject);
		}
	}
}
