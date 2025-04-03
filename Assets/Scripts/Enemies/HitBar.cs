using Unity.VisualScripting;
using UnityEngine;

public class HitBar : MonoBehaviour
{
	[Header("Shake")]
	[SerializeField] float shakeDuration = 0.2f;
	[SerializeField] float shakeAmplitude = 5f;
	[SerializeField] float shakeStrength = 0.5f;

	[Header("Transition")]
	[SerializeField] float scaleDuration = 0.05f;
	
	[Header("Colors")]
	[SerializeField] Color backgroundColor = Color.black;
	[SerializeField] Color currentColor = Color.white;
	[SerializeField] Color nextColor = Color.gray;

	[Header("Sprites")]
	[SerializeField] Sprite backgroundSprite;
	[SerializeField] Sprite hitACurrentSprite;
	[SerializeField] Sprite hitANextSprite;
	[SerializeField] Sprite hitBCurrentSprite;
	[SerializeField] Sprite hitBNextSprite;
	[SerializeField] Sprite hitCCurrentSprite;
	[SerializeField] Sprite hitCNextSprite;

	[Header("Sprites Outline")]
	[SerializeField] Sprite backgroundOutlineSprite;
	[SerializeField] Sprite hitACurrentOutlineSprite;
	[SerializeField] Sprite hitANextOutlineSprite;
	[SerializeField] Sprite hitBCurrentOutlineSprite;
	[SerializeField] Sprite hitBNextOutlineSprite;
	[SerializeField] Sprite hitCCurrentOutlineSprite;
	[SerializeField] Sprite hitCNextOutlineSprite;

	[Header("Sprite Renderers")]
	[SerializeField] SpriteRenderer backgroundSR;
	[SerializeField] SpriteRenderer backgroundOutlineSR;
	[SerializeField] SpriteRenderer currentSR;
	[SerializeField] SpriteRenderer currentOutlineSR;
	[SerializeField] SpriteRenderer nextSR;
	[SerializeField] SpriteRenderer nextOutlineSR;

	float _shakeElapsedTime = 0f;
	float _scaleElapsedTime = 0f;

	Sprite nextSprite;
	Sprite nextOutlineSprite;
	Sprite nextNextSprite;
	Sprite nextNextOutlineSprite;

	private void Start()
	{
		backgroundSR.sprite = backgroundSprite;
		backgroundOutlineSR.sprite = backgroundOutlineSprite;

		backgroundSR.color = backgroundColor;
	}

	private void Update()
	{
		//HandleShake();

		HandleScale();

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

			backgroundSR.transform.localPosition = new(shakeX, shakeY, backgroundSR.transform.localPosition.z);
			backgroundOutlineSR.transform.localPosition = new(shakeX, shakeY, backgroundOutlineSR.transform.localPosition.z);
			currentSR.transform.localPosition = new(shakeX, shakeY, currentSR.transform.localPosition.z);
			currentOutlineSR.transform.localPosition = new(shakeX, shakeY, currentOutlineSR.transform.localPosition.z);
			nextSR.transform.localPosition = new(shakeX, shakeY, nextSR.transform.localPosition.z);
			nextOutlineSR.transform.localPosition = new(shakeX, shakeY, nextOutlineSR.transform.localPosition.z);
		}
		else
		{
			backgroundSR.transform.localPosition = new(0f, 0f, backgroundSR.transform.localPosition.z);
			backgroundOutlineSR.transform.localPosition = new(0f, 0f, backgroundOutlineSR.transform.localPosition.z);
			currentSR.transform.localPosition = new(0f, 0f, currentSR.transform.localPosition.z);
			currentOutlineSR.transform.localPosition = new(0f, 0f, currentOutlineSR.transform.localPosition.z);
			nextSR.transform.localPosition = new(0f, 0f, nextSR.transform.localPosition.z);
			nextOutlineSR.transform.localPosition = new(0f, 0f, nextOutlineSR.transform.localPosition.z);
		}
	}

	void HandleScale()
	{
		if (_scaleElapsedTime < scaleDuration)
		{
			float t = _scaleElapsedTime / scaleDuration;

			currentSR.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			currentOutlineSR.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			nextSR.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
			nextOutlineSR.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);

			currentSR.color = Color.Lerp(currentColor, new Color(1f, 1f, 1f, 0f), t);
			nextSR.color = Color.Lerp(nextColor, currentColor, t);

			_scaleElapsedTime += Time.deltaTime;
		}
		else
		{
			currentSR.transform.localScale = Vector3.one;
			currentOutlineSR.transform.localScale = Vector3.one;
			nextSR.transform.localScale = Vector3.one;
			nextOutlineSR.transform.localScale = Vector3.one;

			currentSR.color = currentColor;
			nextSR.color = nextColor;

			currentSR.sprite = nextSprite;
			currentOutlineSR.sprite = nextOutlineSprite;
			nextSR.sprite = nextNextSprite;
			nextOutlineSR.sprite = nextNextOutlineSprite;
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
				nextSprite = hitACurrentSprite;
				nextOutlineSprite = hitACurrentOutlineSprite;
				break;
			case HitType.Type.B:
				nextSprite = hitBCurrentSprite;
				nextOutlineSprite = hitBCurrentOutlineSprite;
				break;
			case HitType.Type.C:
				nextSprite = hitCCurrentSprite;
				nextOutlineSprite = hitCCurrentOutlineSprite;
				break;
			default:
				nextSprite = null;
				nextOutlineSprite = null;
				break;
		}

		switch (next)
		{
			case HitType.Type.A:
				nextNextSprite = hitANextSprite;
				nextNextOutlineSprite = hitANextOutlineSprite;
				break;
			case HitType.Type.B:
				nextNextSprite = hitBNextSprite;
				nextNextOutlineSprite = hitBNextOutlineSprite;
				break;
			case HitType.Type.C:
				nextNextSprite = hitCNextSprite;
				nextNextOutlineSprite = hitCNextOutlineSprite;
				break;
			default:
				nextNextSprite = null;
				nextNextOutlineSprite = null;
				break;
		}

		_scaleElapsedTime = 0f;
	}
}
