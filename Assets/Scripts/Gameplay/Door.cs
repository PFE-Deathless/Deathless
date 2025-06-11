using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
	[Header("Parameters")]
	[SerializeField] float fadeDuration = 1f;
	[SerializeField, Tooltip("Type of door (Lever : opened by a lever, Progression : Opened by a finished dungeon, Key : Opened by a key)")] Type doorType = Type.Key;
	[SerializeField, Tooltip("Dungeon that opens the door if finished (If Progression is selected in Door Type)")] Dungeon dungeonValidation;

	[Header("Technical")]
	[SerializeField] ParticleSystem smokePS;
	[SerializeField] GameObject doorMesh;

	public bool FinishedActivation { get; set; }

	Material _transparentMaterial;
	Color _baseColor;
	bool _activated = false;
	float _elapsedTime = 0f;

	private void Start()
	{
		if (doorType == Type.Progression && GameManager.Instance.IsUnlocked(dungeonValidation))
			Destroy(gameObject);

		_transparentMaterial = GetComponentInChildren<MeshRenderer>().material;
		_baseColor = _transparentMaterial.GetColor("_Base_Color");
	}

	private void Update()
	{
		if (_activated)
		{
			if (_elapsedTime < fadeDuration)
			{
				float percentage = 1f - (_elapsedTime / fadeDuration);

				// Color change
				_baseColor.a = percentage;
				_transparentMaterial.SetColor("_Base_Color", _baseColor);

				// Shake
                float strength = percentage * 0.5f;
                float shakeX = (Mathf.PerlinNoise(Time.time * 20f, 0) - 0.5f) * 2f * strength;
                float shakeZ = (Mathf.PerlinNoise(0, Time.time * 20f) - 0.5f) * 2f * strength;
				doorMesh.transform.localPosition = new(shakeX, 0f, shakeZ);

                _elapsedTime += Time.deltaTime;
			}
			else
			{
				if (!FinishedActivation)
				{
					smokePS.Stop();
                    doorMesh.SetActive(false);

                    GetComponentInChildren<Collider>().enabled = false;
				}

				FinishedActivation = true;
			}
		}
	}

	public void Activate(bool playAnimation = true)
	{
		if (!_activated && playAnimation)
		{
			smokePS.Play();
			CameraBehavior.Instance.Shake(0.4f, 100f, fadeDuration * 1.5f);
        }

		_activated = true;
	}

	public enum Type
	{
		Key,
		Progression,
	}
}
