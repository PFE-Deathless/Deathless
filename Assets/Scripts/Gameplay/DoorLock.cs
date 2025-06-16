using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class DoorLock : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField] InteractableType interactableType = InteractableType.Interact;
	[SerializeField] Door door;

	[Header("Animation")]
	[SerializeField] float animDuration = 0.5f;
	[SerializeField] AnimationCurve animCurve;

	[Header("Technical")]
	[SerializeField] GameObject padlockMesh;

	private bool _activated = false;
	private float _elapsedTime = 0f;

    private void Update()
    {
        HandleActivation();
    }

    public void Interact(InteractableType type = InteractableType.Both)
	{
		if (_activated)
			return;

		if (interactableType == type || interactableType == InteractableType.Both)
		{
			if (GameManager.Instance.UseKey())
			{
				_activated = true;
				door.Activate();
				GetComponent<Collider>().enabled = false;
			}
		}
	}

	void HandleActivation()
	{
        if (!_activated)
            return;

		if (_elapsedTime < animDuration)
		{
            padlockMesh.transform.localScale = Vector3.one * animCurve.Evaluate(_elapsedTime / animDuration);
			_elapsedTime += Time.deltaTime;
        }
		else
		{
            Destroy(gameObject);
        }
    }
}
