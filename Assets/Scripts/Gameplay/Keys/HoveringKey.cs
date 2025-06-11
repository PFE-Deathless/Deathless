using UnityEngine;

public class HoveringKey : MonoBehaviour
{
	[Header("Rotation & Hovering")]
	[SerializeField] float rotationSpeed = 120f;
	[SerializeField] float hoveringSpeed = 2f;
	[SerializeField] float hoveringDelta = 0.2f;
	[SerializeField] AnimationCurve hoveringCurve;

	[Header("Pick Up")]
	[SerializeField] float pickUpDuration = 0.5f;
	[SerializeField] AnimationCurve pickUpCurve;

	[Header("Technical")]
	[SerializeField] Transform keyMeshTransform;
	[SerializeField] GameObject particleFeedbackPrefab;

	private float _hoveringDuration = 0f;
	private float _hoveringElapsedTime;

	private float _pickUpElapsedTime = 0f;
	private bool _isPickedUp = false;

	private void Start()
	{
		_hoveringDuration = 1f / hoveringSpeed;
	}

	private void Update()
	{
		// Pick up
		if (_isPickedUp)
		{
			if (_pickUpElapsedTime < pickUpDuration)
			{
				float t = pickUpCurve.Evaluate(_pickUpElapsedTime / pickUpDuration);
				float s = 1f - t;
				keyMeshTransform.localScale = new(s, s, s);
				keyMeshTransform.position = Vector3.LerpUnclamped(transform.position, PlayerController.Instance.transform.position, t);

				_pickUpElapsedTime += Time.deltaTime;
			}
			else
			{
				GameManager.Instance.AddKey();
				Destroy(gameObject);
			}
		}

		// Hovering
		if (_hoveringElapsedTime < _hoveringDuration)
		{
			Vector3 pos = keyMeshTransform.localPosition;
			pos.y = hoveringCurve.Evaluate(_hoveringElapsedTime / _hoveringDuration) * hoveringDelta;
			keyMeshTransform.localPosition = pos;
			_hoveringElapsedTime += Time.deltaTime;
		}
		else
		{
			_hoveringElapsedTime -= _hoveringDuration;
		}

		// Rotation
		keyMeshTransform.localRotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);

	}

	private void OnTriggerEnter(Collider other)
	{
		GetComponent<Collider>().enabled = false;
		Instantiate(particleFeedbackPrefab, transform.position, Quaternion.identity, keyMeshTransform);
		_isPickedUp = true;
	}
}
