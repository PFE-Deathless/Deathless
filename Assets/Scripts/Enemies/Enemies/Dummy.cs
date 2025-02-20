using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] int healthMax = 3;

	[Header("Respawn")]
	[SerializeField] AnimationCurve scaleCurve;
	[SerializeField, Tooltip("Time it will take to shrink AND grow back")] float scaleDuration = 0.3f;

	public HitType.Type CurrentType { get; private set; }
	public HitType.Type[] Types { get; private set; }

	int _health;

	HitBar _hitBar;

	private void Start()
	{
		_hitBar = GetComponentInChildren<HitBar>();
		SetTypes();
		_health = healthMax;
	}

	public void TakeDamage()
	{
		_health--;
		if (_health <= 0)
		{
			StartCoroutine(Respawn());
			return;
		}

		CurrentType = Types[healthMax - _health];
		_hitBar.UpdateHitBar(healthMax - _health);
	}


	void SetTypes()
	{
		Types = new HitType.Type[healthMax];
		for (int i = 0; i < healthMax; i++)
		{
			Types[i] = HitType.GetRandomType();
		}
		_hitBar.SetTypes(Types);

		CurrentType = Types[0];
	}

	IEnumerator Respawn()
	{
		float elapsedTime = 0f;
		float duration = scaleDuration;

		GetComponent<Collider>().enabled = false;

		// Shrink
		while (elapsedTime < duration)
		{
			transform.localScale = scaleCurve.Evaluate(elapsedTime / duration) * Vector3.one;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_health = healthMax;
		SetTypes();

		elapsedTime = 0f;
		duration = scaleDuration;

		// Grow
		while (elapsedTime < duration)
		{
			transform.localScale = scaleCurve.Evaluate(1f - (elapsedTime / duration)) * Vector3.one;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localScale = Vector3.one;
		GetComponent<Collider>().enabled = true;
	}
}
