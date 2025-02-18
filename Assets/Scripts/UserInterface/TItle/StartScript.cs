using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public string SceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }


}
