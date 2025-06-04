using UnityEngine;
using TMPro;

public class SoulsDisplay : MonoBehaviour
{
	public static SoulsDisplay Instance { get; private set; }

	[SerializeField] GameObject soulTuto;
	[SerializeField] GameObject soulDungeon1;
	[SerializeField] GameObject soulDungeon2;
	[SerializeField] GameObject soulDungeon3;
	[SerializeField] GameObject soulDungeon4;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void UpdateSouls()
	{
		Debug.Log("paf");
		soulTuto.SetActive(GameManager.Instance.playerData.tutorialSoul && !GameManager.Instance.playerData.tutorial);
		soulDungeon1.SetActive(GameManager.Instance.playerData.dungeon1Soul && !GameManager.Instance.playerData.dungeon1);
		soulDungeon2.SetActive(GameManager.Instance.playerData.dungeon2Soul && !GameManager.Instance.playerData.dungeon2);
		soulDungeon3.SetActive(GameManager.Instance.playerData.dungeon3Soul && !GameManager.Instance.playerData.dungeon3);
		soulDungeon4.SetActive(GameManager.Instance.playerData.dungeon4Soul && !GameManager.Instance.playerData.dungeon4);
	}
}
