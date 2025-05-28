using UnityEngine;

public class EffectArea : MonoBehaviour
{
	float _duration;
	float _elapsedTime;

	bool _disabled = false;

	public void Setup(float radius, float duration)
	{
		gameObject.transform.localScale = Vector3.one * radius * 2f;
		_duration = duration;
	}

	private void Update()
	{
		if (_elapsedTime < _duration)
		{
			_elapsedTime += Time.deltaTime;
		}
		else if (!_disabled)
		{
			_disabled = true;
			GetComponent<Collider>().enabled = false;
			Destroy(gameObject, _duration);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerHealth p = other.gameObject.GetComponentInParent<PlayerHealth>();
		if (p != null)
			p.TakeDamage(1);
	}
}
