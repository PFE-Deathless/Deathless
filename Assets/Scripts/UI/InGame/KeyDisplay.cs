using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class KeyDisplay : MonoBehaviour
{
	public static KeyDisplay Instance { get; private set; }

	[SerializeField] TextMeshProUGUI keyNumberTMP;
	[SerializeField] Transform keyNumberTransform;
	[SerializeField] AnimationCurve scaleCurve;
	[SerializeField] AnimationCurve colorCurve;
	[SerializeField] float animationDuration = 0.4f;
	[SerializeField] Color colorChange;

	float _elapsedTime = 0f;
	Color _defaultColor;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		_defaultColor = keyNumberTMP.color;
	}

	private void Update()
	{
		if (_elapsedTime > 0f)
		{
			float percentage = 1f - (_elapsedTime / animationDuration);
			keyNumberTransform.localScale = Vector3.one * scaleCurve.Evaluate(percentage);
			keyNumberTMP.color = Color.Lerp(_defaultColor, colorChange, colorCurve.Evaluate(percentage));
			_elapsedTime -= Time.deltaTime;
		}
	}

	public void AddKey(int number)
	{
		_elapsedTime = animationDuration;
		keyNumberTMP.text = number.ToString();
	}

	public void RemoveKey(int number)
	{
		keyNumberTMP.text = number.ToString();
	}

	public void SetKeyNumber(int number)
	{
		keyNumberTMP.text = number.ToString();
	}
}
