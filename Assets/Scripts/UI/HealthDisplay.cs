using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	// ######## STATISTICS ########
	//public int health;
	//public int maxHealth;

	[Header("Appearance")]

	public Sprite emptyHeart;
	public Sprite fullHeart;
	public Image[] hearts;

	//[Header("References")]
	//public PlayerHealth playerHealth;


	//void Start()
	//{

	//}

	public void UpdateHealth(int value) // La valeur passée en paramètre est la quantité de HP actuel du joueur
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].sprite = i < value ? fullHeart : emptyHeart; // si il a de la vie les coeur sont pleins, sinon ils sont vides
			/* équivalent à faire 
			
			if (i < value)
			{
				hearts[i].sprite = fullHeart;
			}
			else
			{
				hearts[i].sprite = emptyHeart;
			}

			*/
		}
	}

	public void UpdateHealthMax(int value) // La valeur passée en paramètre est la quantité de HP max du joueur
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].enabled = i < value;

			/* équivalent à faire 
			
			if (i < value)
			{
				hearts[i].enabled = true;
			}
			else
			{
				hearts[i].enabled = false;
			}

			*/
		}
	}

	//void Update()
	//{
	//	// pour debug :emojinerd:
	//	//playerHealth.health = health;
	//	//playerHealth.healthMax = maxHealth;

	//	for (int i = 0; i < hearts.Length; i++)
	//	{
	//		if (i < playerHealth.health) // si il a de la vie les coeur sont pleins, sinon ils sont vides
	//		{
	//			hearts[i].sprite = fullHeart;
	//		}
	//		else
	//		{
	//			hearts[i].sprite = emptyHeart;
	//		}

	//		if ( i < playerHealth.healthMax) 
	//		{
	//			hearts[i].enabled = true;
	//		}
	//		else
	//		{
	//			hearts[i].enabled = false;
	//		}
	//	}
	//}

}
