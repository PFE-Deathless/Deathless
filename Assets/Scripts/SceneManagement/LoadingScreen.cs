using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[Header("Technical")]
	[SerializeField] RawImage fadeImage;
	[SerializeField] Slider progressBar;

	public bool IsFadingIn => _isFadingIn;
	public bool IsFadingOut => _isFadingOut;

	// Private properties
	Color _color;
	float _fadeInDuration = 0.5f;
	float _fadeOutDuration = 0.5f;
	bool _isFadingIn = false;
	bool _isFadingOut = false;

	private void Start()
	{
		_color = fadeImage.color;
	}

	public void SetTiming(float fadeInDuration,  float fadeOutDuration)
	{
		_fadeInDuration = fadeInDuration;
		_fadeOutDuration = fadeOutDuration;
	}

	public void SetProgressBarValue(float value)
	{
		progressBar.value = value;
	}

	public void FadeIn()
	{
		StartCoroutine(FadeInCoroutine());
	}

	public void FadeOut()
	{
		StartCoroutine(FadeOutCoroutine());
	}

	IEnumerator FadeInCoroutine()
	{
		_isFadingIn = true;

		float elapsedTime = 0f;

		while (elapsedTime < _fadeInDuration)
		{
			_color.a = (elapsedTime / _fadeInDuration);
			fadeImage.color = _color;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_color.a = 1f;
		fadeImage.color = _color;

		_isFadingIn = false;
	}

	IEnumerator FadeOutCoroutine()
	{
		_isFadingOut = true;

		float elapsedTime = 0f;

		while (elapsedTime < _fadeOutDuration)
		{
			_color.a = 1f - (elapsedTime / _fadeOutDuration);
			fadeImage.color = _color;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_color.a = 0f;
		fadeImage.color = _color;

		_isFadingOut = false;
	}
}
