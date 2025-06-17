using UnityEngine;

public class HoveringRock : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float rotationSpeedMin = 20f;
	[SerializeField] float rotationSpeedMax = 70f;
	[SerializeField] float verticalDisplacementSpeedMin = 0.1f;
	[SerializeField] float verticalDisplacementSpeedMax = 0.5f;
	[SerializeField] float verticalDisplacementMinDelta = 0.3f;
	[SerializeField] float verticalDisplacementMaxDelta = 0.8f;
	[SerializeField] AnimationCurve verticalDisplacementCurve;

	Vector3 _originalPos;

	Vector3 _randomAxis;

	float _randomRotationSpeed;
	float _randomDisplacementDelta;

	float _verticalDuration;
	float _verticalElapsedTime = 0f;

	private void Start()
	{
		_verticalDuration = 1f / Random.Range(verticalDisplacementSpeedMin, verticalDisplacementSpeedMax);
		_originalPos = transform.position;
		_randomAxis =  Random.onUnitSphere;
		_randomRotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax) * (Random.value > 0.5f ? 1f : -1f);
		_randomDisplacementDelta = Random.Range(verticalDisplacementMinDelta, verticalDisplacementMaxDelta);
	}

	private void Update()
	{
		if (_verticalElapsedTime > _verticalDuration)
			_verticalElapsedTime -= _verticalDuration;
		transform.position = _originalPos + new Vector3(0f, verticalDisplacementCurve.Evaluate(_verticalElapsedTime / _verticalDuration) * _randomDisplacementDelta, 0f);
		_verticalElapsedTime += Time.deltaTime;

		transform.Rotate(_randomAxis, _randomRotationSpeed * Time.deltaTime, Space.Self);
	}
}
