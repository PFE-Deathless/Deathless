using UnityEngine;

public class HitBar : MonoBehaviour
{
	//[Header("Amount")]
	int amount;

	[Header("Technical")]
	public float offset = 1f;
	public float size = 0.3f;
	public float scale = 2f;
	public GameObject currentSprite;
	
	GameObject[] hits;
	HitType.Type[] types;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		currentSprite.SetActive(true);
	}

	public void SetTypes(HitType.Type[] t)
	{
		amount = t.Length;
		types = new HitType.Type[amount];
		for (int i = 0; i < amount; i++)
		{
			types[i] = t[i];
		}
		ApplyTypes();
	}

	public void UpdateHitBar(int current)
	{
		for (int i = 0; i < amount; i++)
		{
			if (i < current)
			{
				hits[i].GetComponent<SpriteRenderer>().sprite = null;
			}
		}
		currentSprite.transform.localPosition = hits[current].transform.localPosition;
	}

	void ApplyTypes()
	{
		if (hits != null)
		{
			for (int i = 0;i < hits.Length;i++)
			{
				Destroy(hits[i]);
			}
		}

		hits = new GameObject[amount];

		for (int i = 0; i < amount; i++)
		{
			hits[i] = new GameObject("Sprite " + (i + 1));
			hits[i].transform.parent = transform;
			hits[i].transform.localScale = new Vector3(scale, scale, scale);
			hits[i].transform.localPosition = new Vector3(((size + offset) * i) - ((size + offset) * (amount - 1)) / 2f, 0f, 0f);
			SpriteRenderer s = hits[i].AddComponent<SpriteRenderer>();
			s.sprite = HitType.GetSprite(types[i]);
		}

		currentSprite.transform.localPosition = hits[0].transform.localPosition;
	}
}
