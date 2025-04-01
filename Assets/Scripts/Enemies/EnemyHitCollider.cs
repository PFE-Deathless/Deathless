using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			PlayerHealth.Instance.TakeDamage(1);
			gameObject.SetActive(false);
		}
	}
}
