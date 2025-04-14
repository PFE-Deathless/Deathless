using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float duration = 2f;
	[SerializeField] float displacement = 2f;

	[Header("Technical")]
	[SerializeField] SpriteRenderer spriteRenderer;

	private float _elapsedTime = 0f;
	private Color _originalColor;

	private void Start()
	{
		_originalColor = spriteRenderer.color;
	}

	private void Update()
	{
		if (_elapsedTime < duration)
		{
			float value = _elapsedTime / duration;

			spriteRenderer.transform.localPosition = new(0f, displacement * value, 0f);

			_originalColor.a = 1f - (value * value);
			spriteRenderer.color = _originalColor;

			_elapsedTime += Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
