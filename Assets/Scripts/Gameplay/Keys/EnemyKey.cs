using UnityEngine;

public class EnemyKey : MonoBehaviour
{
	[Header("Pick Up")]
	[SerializeField] float pickUpDuration = 0.5f;
	[SerializeField] AnimationCurve pickUpCurve;

	[Header("Technical")]
	[SerializeField] Transform keyMeshTransform;

	private float _pickUpElapsedTime = 0f;
	private bool _isDropped = false;

	private void Update()
	{
		// Pick up
		if (_isDropped)
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
	}

	public void DropKey()
	{
		transform.parent = null;
		_isDropped = true;
	}
}
