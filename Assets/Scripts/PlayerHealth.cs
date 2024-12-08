using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public int healthMax = 10;

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
		}
	}
}
