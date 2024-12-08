using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int healthMax = 3;
	
	public HitType.Type currentType { get; private set; }
	public HitType.Type[] types { get; private set; }

	int health;
	HitBar hitBar;

	void Start()
	{
		hitBar = GetComponentInChildren<HitBar>();
		SetTypes();
		currentType = types[0];
		health = healthMax;
	}

	public void TakeDamage()
	{
		health--;
		if (health <= 0)
		{
			Destroy(gameObject);
			return;
		}

		currentType = types[healthMax - health];
		hitBar.UpdateHitBar(healthMax - health);

	}

	void SetTypes()
	{
		types = new HitType.Type[healthMax];
		for (int i = 0; i < healthMax; i++)
		{
			types[i] = HitType.GetRandomType();
		}
		hitBar.SetTypes(types);
	}
}
