using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	[SerializeField, Tooltip("Update per second")] float updateFrequency = 10f;

	TextMeshProUGUI _fpsText;
	float _elapsedTime;
	float _updateDuration;

	private void Start()
	{
		_fpsText = GetComponentInChildren<TextMeshProUGUI>();
		_updateDuration = 1f / updateFrequency;
		_elapsedTime = _updateDuration;

	}

	private void Update()
	{
		if (_elapsedTime < _updateDuration)
		{
			_elapsedTime += Time.deltaTime;
		}
		else
		{
			_elapsedTime = 0f;
			_fpsText.text = (1f / Time.deltaTime).ToString("#.0");
		}
	}
}
