using Unity.VisualScripting;
using UnityEngine;

public class CurveProjectile : MonoBehaviour
{
	[SerializeField] GameObject previsualisationPrefab;
	[SerializeField] GameObject effectPrefab;

	private CurveShooter.Curve _curve;
	private ProjectilePrevisualisation _previsualisation;
	private float _duration;
	private float _elapsedTime = 0f;

	public void Setup(CurveShooter.Curve curve, float duration)
	{
		_curve = curve;
		_duration = duration;
		GameObject obj = Instantiate(previsualisationPrefab, curve.Evaluate(0.99f), Quaternion.identity);
		_previsualisation = obj.GetComponent<ProjectilePrevisualisation>();
	}

	void Update()
	{
		if (_elapsedTime < _duration)
		{
			float t = _elapsedTime / _duration;
			transform.position = _curve.Evaluate(t);
			_previsualisation.SetSize(t);
		}
		else
		{
			Destroy(_previsualisation.gameObject);
		}
	}
}
