using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	// ######## STATISTICS ########
	public int health;
	public int maxHealth;

	[Header("Appearance")]

	public Sprite emptyHeart;
	public Sprite fullHeart;
	public Image[] hearts;

	[Header("References")]
	public PlayerHealth playerHealth;


	void Start()
	{
		playerHealth.health = health;
		playerHealth.healthMax = maxHealth;
	}

	void Update()
	{
		// pour debug :emojinerd:
		//playerHealth.health = health;
		//playerHealth.healthMax = maxHealth;

		for (int i = 0; i < hearts.Length; i++)
		{
			if (i < playerHealth.health) // si il a de la vie les coeur sont pleins, sinon ils sont vides
			{
				hearts[i].sprite = fullHeart;
			}
			else
			{
				hearts[i].sprite = emptyHeart;
			}

			if ( i < playerHealth.healthMax) 
			{
				hearts[i].enabled = true;
			}
			else
			{
				hearts[i].enabled = false;
			}
		}
	}

}
