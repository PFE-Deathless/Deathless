using UnityEngine;

public class InteractObject : MonoBehaviour
{
	public static InteractObject Instance { get; private set; }

	[Header("Properties")]
	[SerializeField] float rotationSpeed = 60f;
	[SerializeField] float verticalDisplacementSpeed = 2f;
	[SerializeField] float verticalDisplacementDelta = 0.2f;
	[SerializeField] AnimationCurve verticalDisplacementCurve;

	[Header("Technical")]
	[SerializeField] Transform pivot;

	float _verticalDuration;
	float _verticalElapsedTime = 0f;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		_verticalDuration = 1f / verticalDisplacementSpeed;
	}

	private void Update()
	{
		if (_verticalElapsedTime > _verticalDuration)
			_verticalElapsedTime -= _verticalDuration;
		pivot.localPosition = new(0f, verticalDisplacementCurve.Evaluate(_verticalElapsedTime / _verticalDuration) * verticalDisplacementDelta, 0f);
		_verticalElapsedTime += Time.deltaTime;

		pivot.localEulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
