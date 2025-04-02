using UnityEngine;
using UnityEngine.UIElements;

public class HitBar : MonoBehaviour
{
	[Header("Shake")]
	[SerializeField] float shakeDuration = 0.2f;
	[SerializeField] float shakeAmplitude = 5f;
	[SerializeField] float shakeStrength = 0.5f;


	[Header("Sprites")]
	[SerializeField] Sprite backgroundSprite;
	[SerializeField] Sprite hitACurrentSprite;
	[SerializeField] Sprite hitANextSprite;
	[SerializeField] Sprite hitBCurrentSprite;
	[SerializeField] Sprite hitBNextSprite;
	[SerializeField] Sprite hitCCurrentSprite;
	[SerializeField] Sprite hitCNextSprite;

	[Header("Sprite Renderers")]
	[SerializeField] SpriteRenderer backgroundSR;
	[SerializeField] SpriteRenderer currentSR;
	[SerializeField] SpriteRenderer nextSR;

	float _shakeElapsedTime = 0f;

	private void Start()
	{
		backgroundSR.sprite = backgroundSprite;
	}

	private void Update()
	{
		HandleShake();
	}

	public void Shake()
	{
		_shakeElapsedTime = shakeDuration;
	}

	void HandleShake()
	{
		if (_shakeElapsedTime >= 0)
		{
			float strength = (_shakeElapsedTime / shakeDuration) * shakeStrength;
			float shakeX = (Mathf.PerlinNoise(Time.time * shakeAmplitude, 0) - 0.5f) * 2f * strength;
			float shakeY = (Mathf.PerlinNoise(0, Time.time * shakeAmplitude) - 0.5f) * 2f * strength;
			_shakeElapsedTime -= Time.deltaTime;

			backgroundSR.transform.localPosition = new(shakeX, shakeY, 0.01f);
			currentSR.transform.localPosition = new(shakeX, shakeY, 0f);
			nextSR.transform.localPosition = new(shakeX, shakeY, -0.01f);
		}
		else
		{
			backgroundSR.transform.localPosition = new(0f, 0f, 0.01f);
			currentSR.transform.localPosition = Vector3.zero;
			nextSR.transform.localPosition = new(0f, 0f, -0.01f);
		}
	}

	public void SetTypes(HitType.Type[] types, int index)
	{
		SetTypes(types[index], types.Length > index + 1 ? types[index + 1] : HitType.Type.None);
	}

	public void SetTypes(HitType.Type current, HitType.Type next)
	{
		switch (current)
		{
			case HitType.Type.A:
				currentSR.sprite = hitACurrentSprite;
				break;
			case HitType.Type.B:
				currentSR.sprite = hitBCurrentSprite;
				break;
			case HitType.Type.C:
				currentSR.sprite = hitCCurrentSprite;
				break;
			default:
				currentSR.sprite = null;
				break;
		}

		switch (next)
		{
			case HitType.Type.A:
				nextSR.sprite = hitANextSprite;
				break;
			case HitType.Type.B:
				nextSR.sprite = hitBNextSprite;
				break;
			case HitType.Type.C:
				nextSR.sprite = hitCNextSprite;
				break;
			default:
				nextSR.sprite = null;
				break;
		}
	}
}
