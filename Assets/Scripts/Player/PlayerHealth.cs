using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[Header("Statistics")]
	public int healthMax = 5;
	public int health;

	[Tooltip("Invicibility duration after taking a hit")]
	public float invicibilityTime = 0.8f;

	[Header("Technical")]

	[Header("VFX")]
	public ParticleSystem damageParticle;


	[HideInInspector] public bool invicible = false;

	void Start()
	{
		health = healthMax;
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
