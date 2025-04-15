using UnityEngine;

public class EffectArea : MonoBehaviour
{
	public void Setup(float radius, float duration)
	{
		gameObject.transform.localScale = Vector3.one * radius * 2f;
		Destroy(gameObject, duration);
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerHealth p = other.gameObject.GetComponentInParent<PlayerHealth>();
		if (p != null)
			p.TakeDamage(1);
	}
}
