using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{
	public float knockbackForce;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			PlayerHealth.Instance.TakeDamage(1);
			PlayerController.Instance.Knockback((transform.position - PlayerController.Instance.transform.position).normalized * knockbackForce);
			gameObject.SetActive(false);
		}
	}
}
