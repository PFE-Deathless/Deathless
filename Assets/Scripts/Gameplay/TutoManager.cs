using UnityEngine;
using System.Collections;
using System;

public class TutoManager : MonoBehaviour
{
    public static TutoManager Instance { get; private set; }

    // R�f�rences
    [Header("UIs")]
    [SerializeField] GameObject[] infoUI;

    [Header("Autres objets a d�sactiver")]
    [SerializeField] GameObject dummy;
    [SerializeField] GameObject porteAOuvrir;





    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // d�sactive toute les UI au lancement de la sc�ne
        for (int i = 0; i < infoUI.Length; i++)
        {
            infoUI[i].gameObject.SetActive(false);
        }
        // Active la premi�re UI
        StartCoroutine(Affichage());
    }

    void Update()
    {
        // Quand le dummy il meurt la premi�re fois �a ouvre la suite
        if (dummy.GetComponent<Dummy>().Died == true) DummyDies();

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

    void DummyDies()
    {
        porteAOuvrir.SetActive(false);
        infoUI[1].SetActive(false);

    }

}
