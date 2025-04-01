using UnityEngine;
using System.Collections;
using System;

public class TutoManager : MonoBehaviour
{
    public static TutoManager Instance { get; private set; }

    // Références
    [Header("UIs")]
    [SerializeField] GameObject[] infoUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // désactive toute les UI au lancement de la scène
        for (int i = 0; i < infoUI.Length; i++)
        {
            infoUI[i].gameObject.SetActive(false);
        }
        // Active la première UI
        StartCoroutine(Affichage());
    }

    public void Activate(int _id, bool state)
    {
            infoUI[_id].SetActive(state);
    }

    IEnumerator Affichage()
    {
        yield return new WaitForSeconds(0.75f);
        infoUI[0].SetActive(true);
    }


    /*// Références
    [Header("UIs")]
    [SerializeField] GameObject[] infoUI;

    [Header("Salle du dummy")]
    [SerializeField] GameObject dummy;
    [SerializeField] GameObject porteAOuvrir;


    // variables


    void Start()
    {
        // désactive toute les UI au lancement de la scène
        for (int i = 0; i < infoUI.Length; i++)
        {
            infoUI[i].gameObject.SetActive(false);
        }
        // Active la première UI
        StartCoroutine(Affichage());
    }

    void Update()
    {
        // Quand le dummy il meurt la première fois ça ouvre la suite
        if (dummy.GetComponent<Dummy>().Died == true) Ouverture();
        
    }


    IEnumerator Affichage()
    {
        yield return new WaitForSeconds(0.75f);
        infoUI[0].SetActive(true);
    }

    void Ouverture()
    {

    }*/
}
