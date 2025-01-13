using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[Header("Statistics")]
	public int healthMax = 5;
	public int health;

	[Tooltip("Invicibility duration after taking a hit")]
	public float invicibilityTime = 0.8f;

	[Header("Technical")]
	public float blinkDelay = 0.05f;

	[Header("VFX")]
	public ParticleSystem damageParticle;

	// Damage taken
	List<MeshRenderer> meshRenderers = new();
	List<Material> defaultMaterials = new();
	Material blinkingMaterial;
	bool isBlinking = false;
	bool currentBlinkingPhase = true;
	float blinkingTime = 0.25f;
	float currentBlinkingTime;
	float currentBlinkingDelay;


	[HideInInspector] public bool invicible = false;

	[HideInInspector] public GameManager gameManager;

	void Start()
	{
		health = healthMax;
		gameManager.healthDisplay.UpdateHealth(health);
		gameManager.healthDisplay.UpdateHealthMax(healthMax);
		blinkingTime = invicibilityTime;
		GetMeshRenderersAndMaterials();
	}

	void Update()
	{
		HandleBlink();
	}

	void GetMeshRenderersAndMaterials()
	{
		blinkingMaterial = Resources.Load<Material>("Materials/M_BlinkDamage");
		MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer mr in mrs)
		{
			meshRenderers.Add(mr);
			defaultMaterials.Add(mr.material);
		}
	}

	void HandleBlink()
	{
		if (isBlinking)
		{
			if (Time.time <= currentBlinkingTime + blinkingTime)
			{
				currentBlinkingDelay += Time.deltaTime;
				if (currentBlinkingDelay >= blinkDelay)
				{
					currentBlinkingPhase = !currentBlinkingPhase;
                    currentBlinkingDelay = 0f;
                }
				if (currentBlinkingPhase)
				{
					for (int i = 0; i < meshRenderers.Count; i++)
					{
						meshRenderers[i].material = blinkingMaterial;
					}
				}
				else
				{
					for (int i = 0; i < meshRenderers.Count; i++)
					{
						meshRenderers[i].material = defaultMaterials[i];
					}
				}
				
			}
			else
				isBlinking = false;
		}
		else
		{
			currentBlinkingDelay = 0f;
			for (int i = 0; i < meshRenderers.Count; i++)
			{
				meshRenderers[i].material = defaultMaterials[i];
			}
		}
	}

	public void SetInvicibility(bool invicibility)
	{
		invicible = invicibility;
		MeshRenderer m = GetComponentInChildren<MeshRenderer>();
		m.material.color = new Color(m.material.color.r, m.material.color.g, m.material.color.b, invicible ? 0.1f : 1f);
	}

	public void TakeDamage(int damage)
	{
		if (!invicible)
		{
			health -= damage;
			gameManager.healthDisplay.UpdateHealth(health);
			damageParticle.Play();

			// Blink
			currentBlinkingTime = Time.time;
			isBlinking = true;

			StartCoroutine(InvicibilityTime());
			//Debug.Log("HP : " + health);
		}
	}


	IEnumerator InvicibilityTime()
	{
		invicible = true;
		//SetInvicibility(true);
		yield return new WaitForSeconds(invicibilityTime);
		invicible = false;
		//SetInvicibility(false);
	}
}
