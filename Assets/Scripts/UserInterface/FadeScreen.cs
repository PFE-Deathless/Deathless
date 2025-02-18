using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
	public static FadeScreen Instance { get; private set; }

	[SerializeField] Image fadeImage;
	[SerializeField] float fadeDuration = 0.5f;

	private bool _fadingDone;

	public float FadeDuration => fadeDuration;
	public bool FadingDone => _fadingDone;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	public void StartFadeIn()
	{
		StartCoroutine(FadeIn());
	}

	public void StartFadeOut()
	{
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeIn()
	{
		float elapsedTime = 0f;
		_fadingDone = false;

		while (elapsedTime < fadeDuration)
		{
			fadeImage.color = new(0f, 0f, 0f, elapsedTime / fadeDuration);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		fadeImage.color = new(0f, 0f, 0f, 1f);
		_fadingDone = true;
	}

	IEnumerator FadeOut()
	{
		float elapsedTime = 0f;
		_fadingDone = false;

		while (elapsedTime < fadeDuration)
		{
			fadeImage.color = new(0f, 0f, 0f, 1f - (elapsedTime / fadeDuration));

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		fadeImage.color = new(0f, 0f, 0f, 0f);
		_fadingDone = true;
	}
}
