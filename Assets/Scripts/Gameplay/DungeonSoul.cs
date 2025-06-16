using UnityEngine;

public class DungeonSoul : MonoBehaviour, IInteractable
{
	[Header("Properties")]
	[SerializeField, Tooltip("Type of interaction the player need to do to take the soul")] InteractableType interactableType = InteractableType.Hit;
	[SerializeField, Tooltip("Dungeon that this soul will validate after being reaped by the player")] Dungeon dungeonValidation;
	[SerializeField, ColorUsage(false, true)] Color soulColor;

	[Header("Technical")]
	[SerializeField, Tooltip("Door that will be opened after the soul is reaped")] Door door;
	[SerializeField, Tooltip("Curve that will dictate the scale changes on the soul sphere")] AnimationCurve activationCurve;
	[SerializeField, Tooltip("Duration of the scale animation")] float animationDuration = 1f;
	[SerializeField, Tooltip("Particle prefab that will be spawnded from the souls to the door")] GameObject soulParticleFeedbackPrefab;
	[SerializeField, Tooltip("Mesh of the soul (for scale animation)")] Transform soulMesh;
	[SerializeField, Tooltip("ParticleSystem permanent")] ParticleSystem permanentParticle;
	[SerializeField, Tooltip("ParticleSystem for the slash")] ParticleSystem slashParticle;
	[SerializeField, Tooltip("ParticleSystem exploding")] ParticleSystem explosionParticle;

	bool _animationStarted = false;
	bool _soulDestroyed = false;
	float _elapsedTime = 0f;

	private void Start()
	{
		GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", soulColor);

		if (GameManager.Instance.IsUnlocked(dungeonValidation))
		{
			if (door != null)
				door.Activate(false);
			Destroy(gameObject);
		}
	}

    private void Update()
    {
        if (_animationStarted)
		{
			if (_elapsedTime < animationDuration)
			{
                soulMesh.localScale = Vector3.one * activationCurve.Evaluate(_elapsedTime / animationDuration);
                _elapsedTime += Time.deltaTime;
            }
			else
			{
				if (!_soulDestroyed)
				{
					_soulDestroyed = true;

                    if (door != null)
                        door.Activate();
                    GameManager.Instance.UnlockDungeonSoul(dungeonValidation);
                    PlayerInteract.Instance.Remove(transform);

					if (door != null)
					{
						GameObject obj = Instantiate(soulParticleFeedbackPrefab, transform.position, Quaternion.identity);
						obj.GetComponent<SoulParticleFeedback>().Setup(transform.position, door.transform.position);
					}

                    Destroy(gameObject, 5f);
                }
			}
		}
    }

    public void Interact(InteractableType type)
	{
		if ((interactableType != type && interactableType != InteractableType.Both) || _animationStarted)
			return;

        CameraBehavior.Instance.Shake(0.3f, 300f, 5f);
        permanentParticle.Stop();
        slashParticle.Play();
        explosionParticle.Play();

		_animationStarted = true;
    }
}
