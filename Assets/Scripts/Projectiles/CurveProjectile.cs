using UnityEngine;
using static CurveShooter;

public class CurveProjectile : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float effectRadius = 1f;
	[SerializeField] float effectDuration = 1f;

	[Header("Technical")]
	[SerializeField] GameObject previsualisationPrefab;
	[SerializeField] GameObject effectPrefab;

	private Curve _curve;
	private ProjectilePrevisualisation _previsualisation;
	private float _duration;
	private float _elapsedTime = 0f;

	public void Setup(Curve curve, float duration)
	{
		_curve = curve;
		_duration = duration;
		GameObject obj = Instantiate(previsualisationPrefab, _curve.Evaluate(0.99f), Quaternion.identity, GameManager.Instance.ProjectileParent);
		_previsualisation = obj.GetComponent<ProjectilePrevisualisation>();
		_previsualisation.Setup(effectRadius);
	}

	void Update()
	{
		if (_elapsedTime < _duration)
		{
			float t = _elapsedTime / _duration;
			transform.SetPositionAndRotation(_curve.Evaluate(t), _curve.EvaluateRotation(t));
			_previsualisation.SetSize(t);
			_elapsedTime += Time.deltaTime;
		}
		else
		{
			GameObject obj = Instantiate(effectPrefab, _curve.Evaluate(0.99f), Quaternion.identity, GameManager.Instance.ProjectileParent);
			obj.GetComponent<EffectArea>().Setup(effectRadius, effectDuration);
			Destroy(_previsualisation.gameObject);
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14)
			PlayerHealth.Instance.TakeDamage(1);
	}
}
