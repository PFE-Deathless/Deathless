using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DungeonGenerate : MonoBehaviour
{
    // Listes publiques
    public List<Salle> allSalles = new List<Salle>();
    public List<Salle> genSalle = new List<Salle>();

    // Liste privée
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
            // Methode TriSalle : trouve les salles qui correspondent aux critères
            TriSalle(S);

            // Choix aléatoire dans la liste des salles correspondantes aux critères
            int i = canSalle.Count;
            int j = Random.Range(0, i);

            //Instantiation de la salle tirée au sort
            Instantiate(canSalle[j].gameObject, S.gameObject.transform.position, S.gameObject.transform.rotation);

            // Vide la liste des salles correspondantes avant de passer à la prochaine salle a remplacer
            canSalle.Clear();

            // Destruction de la salle PlaceHolder
            Destroy(S.gameObject);
        }
    }


    void TriSalle(Salle S)
    {
        foreach (Salle s in allSalles)
        {
            // Si can = true la salle correspond aux critères
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

            // Correspondance de la presence de pièges
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

            // Si la salle correspond elle est ajoutée à la liste canSalle
            if (can == true) canSalle.Add(s);
        }
    }
}
