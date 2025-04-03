using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	[HideInInspector] public static HealthDisplay Instance { get; private set; }

	[Header("Health Points")]

	public Sprite emptyHeart;
	public Sprite fullHeart;
	public Image[] hearts;

	[Header("Vignette")]
	[SerializeField] Image[] hitVignetteImages;
	[SerializeField] float fadeSpeed = 2f;


	private float _vignettePercentage = 0f;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Update()
	{
		UpdateVignette();
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

	public void ShowVignette()
	{
		_vignettePercentage = 1f;
	}

	void UpdateVignette()
	{
		if (_vignettePercentage >= 0f)
		{
			SetVignettePercentage(_vignettePercentage);
			_vignettePercentage -= Time.deltaTime * fadeSpeed;
		}
		else
		{
			SetVignettePercentage(0f);
			_vignettePercentage = 0f;
		}
	}

	public void SetVignettePercentage(float percentage)
	{
		foreach(Image vignette in hitVignetteImages)
		{
			Vector3 scale = new(1f, percentage, 1f);
			vignette.transform.localScale = scale;
		}
	}
}
