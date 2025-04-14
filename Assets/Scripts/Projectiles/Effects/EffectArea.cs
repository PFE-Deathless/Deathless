using UnityEngine;

public class EffectArea : MonoBehaviour
{
	EffectObject effect;
	float elapsedTime = 0f;

	bool started = false;

	public void Setup(EffectObject effect)
	{
		this.effect = effect;
		this.effect.gameObject.transform.localScale = Vector3.one * this.effect.radius * 2f;
		started = true;
	}

	private void Update()
	{
		if (started)
			PerformEffect();
	}

	public void PerformEffect()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > effect.duration)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerHealth p = other.gameObject.GetComponent<PlayerHealth>();
		if (p != null)
			p.TakeDamage(effect.damage);
	}
}
