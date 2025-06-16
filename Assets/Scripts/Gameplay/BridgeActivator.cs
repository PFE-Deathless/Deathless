using UnityEngine;

public class BridgeActivator : MonoBehaviour, IActivable
{
	[SerializeField] Transform bridgePart;
	[SerializeField] float startY = -20f;
	[SerializeField] float endY = 0f;
	[SerializeField] float animationDuration = 2f;
	[SerializeField] AnimationCurve curve;

	private bool _activated = false;
	private float _elapsedTime = 0f;

	public bool FinishedActivation { get; set; }

	void Update()
	{
		if (_activated)
		{
			if (_elapsedTime < animationDuration)
			{
				bridgePart.transform.position = new(bridgePart.transform.position.x, Mathf.Lerp(startY, endY, curve.Evaluate(_elapsedTime / animationDuration)), bridgePart.transform.position.z);

				_elapsedTime += Time.deltaTime;
			}
			else
			{
				bridgePart.transform.position = new(bridgePart.transform.position.x, endY, bridgePart.transform.position.z);
				FinishedActivation = true;
			}
		}
	}

	public void Activate(bool playAnimation = true)
	{
		if (playAnimation)
			CameraBehavior.Instance.Shake(0.5f, 100f, animationDuration * 1.5f);
		_activated = true;
	}
}
