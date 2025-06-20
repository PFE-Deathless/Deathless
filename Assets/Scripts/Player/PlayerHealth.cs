using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[HideInInspector] public static PlayerHealth Instance { get; private set; }

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
	BlinkingMaterials blinkingMaterials;
	bool isBlinking = false;
	bool currentBlinkingPhase = true;
	float blinkingTime = 0.25f;
	float currentBlinkingTime;
	float currentBlinkingDelay;

	[HideInInspector] public bool invicible = false;

	bool _invicible;

	class BlinkingMaterials
	{
		public List<SkinnedMeshRenderer> skinnedMeshRenderers;
		public List<Material[]> defaultMaterials;
		public List<Material[]> blinkingMaterials;
		public Material blinkingMaterial;

		public BlinkingMaterials(Material blinkingMaterial)
		{
			skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
			defaultMaterials = new List<Material[]>();
			blinkingMaterials = new List<Material[]>();
			this.blinkingMaterial = blinkingMaterial;
		}

		public void Add(SkinnedMeshRenderer skinnedMeshRenderer)
		{
			skinnedMeshRenderers.Add(skinnedMeshRenderer);
			defaultMaterials.Add(skinnedMeshRenderer.materials);
			Material[] bs = new Material[skinnedMeshRenderer.materials.Length];
			for (int i = 0; i < bs.Length; i++) bs[i] = blinkingMaterial;
			blinkingMaterials.Add(bs);
		}

		public void Blink(bool state)
		{
			for (int  i = 0; i < skinnedMeshRenderers.Count; i++)
			{
				skinnedMeshRenderers[i].materials = state ? blinkingMaterials[i] : defaultMaterials[i];
			}
		}
	}

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		health = healthMax;
		HealthDisplay.Instance.UpdateHealth(health);
		HealthDisplay.Instance.UpdateHealthMax(healthMax);
		blinkingTime = invicibilityTime;
		GetMeshRenderersAndMaterials();
	}

	void Update()
	{
		HandleBlink();

		if (Input.GetKeyDown(KeyCode.KeypadMinus))
			Kill();

		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			HealFull();
		}

		if (Input.GetKeyDown(KeyCode.KeypadMultiply))
		{
			_invicible = !_invicible;
		}
	}

	void GetMeshRenderersAndMaterials()
	{
		Material blinkingMaterial = Resources.Load<Material>("Materials/M_BlinkDamagePlayer");
		blinkingMaterials = new(blinkingMaterial);

		SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer smr in smrs)
		{
			blinkingMaterials.Add(smr);
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
				blinkingMaterials.Blink(currentBlinkingPhase);
			}
			else
				isBlinking = false;
		}
		else
		{
			currentBlinkingDelay = 0f;
			blinkingMaterials.Blink(false);
		}
	}

	public void SetInvicibility(bool invicibility)
	{
		invicible = invicibility;
	}

	public bool TakeDamage(int damage)
	{
		if (!invicible && !_invicible)
		{
			health -= damage;
            CameraBehavior.Instance.Shake(0.4f, 20f, 0.5f);
			HealthDisplay.Instance.ShowVignette();
			if (health <= 0)
				Kill();
			HealthDisplay.Instance.UpdateHealth(health);
			damageParticle.Play();

			// Blink
			currentBlinkingTime = Time.time;
			isBlinking = true;

			StartCoroutine(InvicibilityTime());

            //Debug.Log("Health : " + health);
            return true;
		}

        return false;
	}

	public void Kill()
	{
        GameManager.Instance.ReloadLevel();
	}

	public void Heal(int amount)
	{
        health += amount;
		if (health > healthMax)
			health = healthMax;
		//Debug.Log("Health : " + health);
        HealthDisplay.Instance.UpdateHealth(health);
    }

	public void HealFull()
	{
		health = healthMax;
		HealthDisplay.Instance.UpdateHealth(health);
	}

	IEnumerator InvicibilityTime()
	{
		invicible = true;
		yield return new WaitForSeconds(invicibilityTime);
		invicible = false;
	}
}
