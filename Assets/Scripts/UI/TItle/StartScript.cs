using UnityEngine;

public class StartScript : MonoBehaviour
{
	[Header("Level Paths")]
	[SerializeField] string hubPath = "Assets/Scenes/Levels/HUB.unity";
	[SerializeField] string tutoPath = "Assets/Scenes/Levels/LD_Tuto.unity";

	[Header("Technical")]
	public GameObject canvaActive;
	public GameObject canvaDeactivate;

	public void LoadCanva()
	{
		canvaActive.SetActive(true);
		canvaDeactivate.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		if (GameManager.Instance.HasDungeonSoul(Dungeon.Tutorial))
			LoadLevel(hubPath);
		else
			LoadLevel(tutoPath);
			
	}

	public void LoadLevel(string path)
	{
		GameManager.Instance.LoadLevel(path);
	}
}
