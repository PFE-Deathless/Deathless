using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float fadeInDuration = 0.5f;
	[SerializeField] float fadeOutDuration = 0.5f;
	[SerializeField] float waitDuration = 1f;

	[Header("Properties")]
	[SerializeField] RawImage fadeImage;

	// Private properties
	float _elapsedTime;
	Color _color;

	private void Start()
	{
		_color = fadeImage.color;
		_elapsedTime = 0f;

	}

	private void Update()
	{
		if (_elapsedTime < fadeInDuration)
		{
			_color.a = (_elapsedTime / fadeInDuration);
		}
		else if (_elapsedTime < fadeInDuration + waitDuration)
		{
			_color.a = 1f;
		}
		else if (_elapsedTime < fadeInDuration + waitDuration + fadeOutDuration)
		{
			_color.a =  1f - ((_elapsedTime + fadeInDuration + waitDuration) / (fadeInDuration + waitDuration + fadeOutDuration));
		}
		else
		{

		}

		_elapsedTime += Time.deltaTime;
		fadeImage.color = _color;
	}
}
