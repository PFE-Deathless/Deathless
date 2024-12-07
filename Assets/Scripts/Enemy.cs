using UnityEngine;

public class Enemy : MonoBehaviour
{

	public HitType.Type type;

	public int health = 3;
	public int healthMax = 3;
	public Sprite healthSprite;
	public SpriteRenderer[] hitPoints;

	void Start()
	{
		GetComponentInChildren<SpriteRenderer>().sprite = HitType.GetSprite(type);
	}

	public void TakeDamage()
	{
		health--;
		UpdateHealthInterface();
		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	public void UpdateHealthInterface()
	{
		for (int i = 0; i < healthMax; i++)
		{
			hitPoints[i].sprite = health - i > 0 ? healthSprite : null;
		}
	}
}
