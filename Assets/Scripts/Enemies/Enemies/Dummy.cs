using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] int healthMax = 3;

	[Header("VFX")]
	public GameObject slashObject;
	public Transform slashTransform;

	[Header("Animator")]
	public Animator animator;

	[Header("Respawn")]
	[SerializeField] AnimationCurve scaleCurve;
	[SerializeField, Tooltip("Time it will take to shrink AND grow back")] float scaleDuration = 0.3f;

	public HitType.Type CurrentType { get; private set; }
	public HitType.Type[] Types { get; private set; }

	int _health;
	bool _died = false;

	public bool Died { get { return _died; } set { _died = value; } }

	HitBar _hitBar;

	private void Start()
	{
		_hitBar = GetComponentInChildren<HitBar>();
		_health = healthMax;
		SetTypes();
	}

	public void TakeDamage()
	{
		animator.SetTrigger("Hit");
		_health--;
		if (slashObject != null && slashTransform != null)
		{
			GameObject obj = Instantiate(slashObject, slashTransform.position, PlayerController.Instance.transform.rotation);
			Destroy(obj, 5f);
		}
		if (_health <= 0)
		{
			_died = true;
			StartCoroutine(Respawn());
			return;
		}

		CurrentType = Types[healthMax - _health];
		_hitBar.SetTypes(Types, healthMax - _health);
		_hitBar.Shake();
	}


	void SetTypes()
	{
		Types = new HitType.Type[healthMax];
		for (int i = 0; i < healthMax; i++)
		{
			Types[i] = HitType.GetRandomType();
		}
		_hitBar.SetTypes(Types, 0);

		CurrentType = Types[0];
	}

	IEnumerator Respawn()
	{
		float elapsedTime = 0f;
		float duration = scaleDuration;

		GetComponent<Collider>().enabled = false;
		animator.SetTrigger("Kill");

		// Shrink
		while (elapsedTime < duration)
		{
			_hitBar.transform.localScale = scaleCurve.Evaluate(elapsedTime / duration) * Vector3.one;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_health = healthMax;
		SetTypes();
		yield return new WaitForSeconds(.5f);
		animator.SetTrigger("Respawn");

		elapsedTime = 0f;
		duration = scaleDuration;

		// Grow
		while (elapsedTime < duration)
		{
			_hitBar.transform.localScale = scaleCurve.Evaluate(1f - (elapsedTime / duration)) * Vector3.one;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_hitBar.transform.localScale = Vector3.one;
		GetComponent<Collider>().enabled = true;
	}
}
