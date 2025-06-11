using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScript : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] TextMeshProUGUI playButtonTMP;
	[SerializeField] GameObject continueButton;
	[SerializeField] GameObject newGameButton;

	[Header("Level Paths")]
	[SerializeField] string hubPath = "Assets/Scenes/Levels/HUB.unity";
	[SerializeField] string tutoPath = "Assets/Scenes/Levels/LD_Tuto.unity";
	[SerializeField] string creditsPath = "Assets/Scenes/Menu/Credits.unity";

	[Header("Technical")]
	[SerializeField] EventSystem eventSystem;

    private void Start()
    {
        continueButton.SetActive(GameManager.Instance.DoesSaveExist);
        eventSystem.SetSelectedGameObject(GameManager.Instance.DoesSaveExist ? continueButton : newGameButton);
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
           eventSystem.SetSelectedGameObject(GameManager.Instance.DoesSaveExist ? continueButton : newGameButton);
    }

	public void Continue()
	{
		StartGame();
    }
	public void NewGame()
	{
		GameManager.Instance.ResetData();
		StartGame();
    }

	public void Credits()
	{
		LoadLevel(creditsPath);
	}

	public void Quit()
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
