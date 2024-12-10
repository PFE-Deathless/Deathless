using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[Header("Statistics")]
	public int healthMax = 10;

	[Tooltip("Invicibility duration after taking a hit")]
	public float invicibilityTime = 0.8f;

	[Header("Technical")]
	public Slider healthSlider;

	[Header("VFX")]
	public ParticleSystem damageParticle;

	int health;

	[HideInInspector] public bool invicible = false;

	void Start()
	{
		health = healthMax;
		healthSlider.value = 1f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			TakeDamage(1);
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
			damageParticle.Play();
			healthSlider.value = (float)health / (float)healthMax;
			StartCoroutine(InvicibilityTime());
			//Debug.Log("HP : " + health);
		}
	}


	IEnumerator InvicibilityTime()
	{
		SetInvicibility(true);
		yield return new WaitForSeconds(invicibilityTime);
		SetInvicibility(false);
	}
}
