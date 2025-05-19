using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class Altar : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField] Dungeon dungeon;
	[SerializeField] InteractableType interactableType = InteractableType.Interact;
	[SerializeField] float cameraSpeed = 20f;
	[SerializeField, ColorUsage(false, true)] Color soulColor;

	[Header("Technical")]
	[SerializeField] Transform[] toActivateObjects;
	[SerializeField] Transform soulTransform;
	[SerializeField] GameObject soulPrefab;

	private bool _activated = false;

	private void Start()
	{
		if (GameManager.Instance.IsUnlocked(dungeon))
			ActivateAll();
	}

	public void Interact(InteractableType type = InteractableType.Both)
	{
		if (_activated)
			return;

		if (interactableType == type || interactableType == InteractableType.Both)
		{
			if (GameManager.Instance.HasDungeonSoul(dungeon))
			{
				GameManager.Instance.UnlockDungeon(dungeon);

				// Here goes the logic to change the UI when consuming the dungeon soul

				StartCoroutine(ActivateObjects());

				gameObject.layer = 0;

				_activated = true;
			}
		}
	}

	public void ActivateAll()
	{
		gameObject.layer = 0;
		_activated = true;
		GameObject obj = Instantiate(soulPrefab, soulTransform.position, soulTransform.rotation, soulTransform);
		obj.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", soulColor);

		if (toActivateObjects.Length == 0)
			return;

		for (int i = 0; i < toActivateObjects.Length; i++)
			toActivateObjects[i].GetComponent<IActivable>().Activate();
	}

	IEnumerator ActivateObjects()
	{
		if (toActivateObjects.Length == 0)
			yield break;

		float elapsedTime;
		Vector3 start = PlayerController.Instance.transform.position;
		Vector3 target = toActivateObjects[0].position;

		InputsManager.Instance.EnableInput(false);

		GameObject follow = new("Follow");

		for (int i = 0; i < toActivateObjects.Length; i++)
		{
			elapsedTime = 0f;

			float distance = Vector3.Distance(start, target);
			float moveDuration = distance / cameraSpeed;

			CameraBehavior.Instance.SetCinematicTarget(follow.transform);

			IActivable activable = toActivateObjects[i].GetComponent<IActivable>();

			while (elapsedTime < moveDuration)
			{
				follow.transform.position = Vector3.Lerp(start, target, elapsedTime / moveDuration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			activable.Activate();

			start = toActivateObjects[i].position;
			if (i < toActivateObjects.Length - 1)
				target = toActivateObjects[i + 1].position;

			while (!activable.FinishedActivation)
				yield return null;

			yield return new WaitForSeconds(0.2f);
		}

		GameObject obj = Instantiate(soulPrefab, soulTransform.position, soulTransform.rotation, soulTransform);
		obj.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", soulColor);

		CameraBehavior.Instance.SetCinematicTarget(null);
		Destroy(follow);

		yield return new WaitForSeconds(0.5f);
		InputsManager.Instance.EnableInput(true);
	}
}
