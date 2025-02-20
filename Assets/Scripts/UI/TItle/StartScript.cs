using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public string SceneName;
    public GameObject canvaActive;
    public GameObject canvaDeactivate;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void LoadCanva()
    {
        canvaActive.SetActive(true);
        canvaDeactivate.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
