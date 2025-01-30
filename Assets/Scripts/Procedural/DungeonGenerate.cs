using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DungeonGenerate : MonoBehaviour
{
    // Listes publiques
    public List<Salle> allSalles = new List<Salle>();
    public List<Salle> genSalle = new List<Salle>();

    // Liste priv�e
    private List<Salle> canSalle = new List<Salle>();

    void Start()
    {       
        ManageLoad(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("ProceduralGym");
        }
    }

    void ManageLoad()
    {
        foreach (Salle S in genSalle)
        {
            // Methode TriSalle : trouve les salles qui correspondent aux crit�res
            TriSalle(S);

            // Choix al�atoire dans la liste des salles correspondantes aux crit�res
            int i = canSalle.Count;
            int j = Random.Range(0, i);

            //Instantiation de la salle tir�e au sort
            Instantiate(canSalle[j].gameObject, S.gameObject.transform.position, S.gameObject.transform.rotation);

            // Vide la liste des salles correspondantes avant de passer � la prochaine salle a remplacer
            canSalle.Clear();

            // Destruction de la salle PlaceHolder
            Destroy(S.gameObject);
        }
    }


    void TriSalle(Salle S)
    {
        foreach (Salle s in allSalles)
        {
            // Si can = true la salle correspond aux crit�res
            bool can = true;

            // Correspondance de la forme de la salle
            switch (S.forme)
            {
                case Forme.carre:
                    if (s.forme != S.forme) can = false;
                    break;

                case Forme.rectangle:
                    if (s.forme != S.forme) can = false;
                    break;
            }

            // Correspondance de la presence d'ennemis
            switch (S.pEnnemis)
            {
                case Presence.Stricte:
                    if (s.pEnnemis != S.pEnnemis) can = false;
                    break;
                case Presence.Nulle:
                    if (s.pEnnemis != S.pEnnemis) can = false;
                    break;
                case Presence.Optionelle:
                    break;
            }

            // Correspondance de la presence de tourelles
            switch (S.pTourelles)
            {
                case Presence.Stricte:
                    if (s.pTourelles != S.pTourelles) can = false;
                    break;
                case Presence.Nulle:
                    if (s.pTourelles != S.pTourelles) can = false;
                    break;
                case Presence.Optionelle:
                    break;
            }

            // Correspondance de la presence de pi�ges
            switch (S.pPieges)
            {
                case Presence.Stricte:
                    if (s.pPieges != S.pPieges) can = false;
                    break;
                case Presence.Nulle:
                    if (s.pPieges != S.pPieges) can = false;
                    break;
                case Presence.Optionelle:
                    break;
            }

            // Si la salle correspond elle est ajout�e � la liste canSalle
            if (can == true) canSalle.Add(s);
        }
    }
}
