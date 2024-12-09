using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public int healthMax = 10;

	[Tooltip("Invicibility duration after taking a hit")]
	public float invicibilityTime = 0.8f;

	int health;

	[HideInInspector] public bool invicible = false;

	void Start()
	{
		health = healthMax;
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
			StartCoroutine(InvicibilityTime());
		}
	}


	IEnumerator InvicibilityTime()
	{
		SetInvicibility(true);
        yield return new WaitForSeconds(invicibilityTime);
        SetInvicibility(false);
    }
}
