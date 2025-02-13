using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	[HideInInspector] public static HealthDisplay Instance { get; private set; }

	[Header("Appearance")]

	public Sprite emptyHeart;
	public Sprite fullHeart;
	public Image[] hearts;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}


	public void UpdateHealth(int value) // La valeur passée en paramètre est la quantité de HP actuel du joueur
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].sprite = i < value ? fullHeart : emptyHeart; // si il a de la vie les coeur sont pleins, sinon ils sont vides
		}
	}

	public void UpdateHealthMax(int value) // La valeur passée en paramètre est la quantité de HP max du joueur
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].enabled = i < value;
		}
	}
}
