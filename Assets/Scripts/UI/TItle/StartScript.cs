using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    [SerializeField] string levelPath;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 0.2f;

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

    public void LoadScene()
    {
        StartCoroutine(LoadFirstScene());
    }

    IEnumerator LoadFirstScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = new(0f, 0f, 0f, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new(0f, 0f, 0f, 1f);

        SceneManager.LoadSceneAsync(levelPath);
    }

}
