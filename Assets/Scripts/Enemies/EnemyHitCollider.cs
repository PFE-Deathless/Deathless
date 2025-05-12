using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{
	public float knockbackForce;
	public float slowDuration;
	public float slowMultiplier;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			if (PlayerHealth.Instance.TakeDamage(1))
			{
				PlayerController.Instance.Knockback((transform.position - PlayerController.Instance.transform.position).normalized * knockbackForce);
				PlayerController.Instance.SetSpeedModifier(slowMultiplier, slowDuration);
			}
			gameObject.SetActive(false);
		}
	}
}
