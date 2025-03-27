using UnityEngine;
using System.Collections;

public class TutoManager : MonoBehaviour
{
    // Références
    [Header("UIs")]
    [SerializeField] GameObject[] infoUI;

    void Start()
    {
        for (int i = 0; i < infoUI.Length; i++)
        {
            infoUI[i].gameObject.SetActive(false);
        }
        StartCoroutine(affichage());
    }

    IEnumerator affichage()
    {
        yield return new WaitForSeconds(0.75f);
        infoUI[0].SetActive(true);
    }
}
