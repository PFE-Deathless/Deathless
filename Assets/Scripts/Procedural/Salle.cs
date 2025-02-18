using UnityEngine;

public class Salle : MonoBehaviour
{
    [Header("Forme")]
    public Forme forme;

    [Header("Type")]
    public Presence pEnnemis;
    public Presence pTourelles;
    public Presence pPieges;

    [Header("Portes")]
    public bool porteNord;
    public bool porteEst;
    public bool porteSud;
    public bool porteOuest;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void OnLoad()
    {
        if(porteNord)
        {

        }

    }

}

public enum Presence 
    { 
        Stricte,     // Je veux une salle avec des ennemis 
        Nulle ,      // Je veux une salle sans ennemis
        Optionelle   // Je veux une salle avec possiblement des ennemis
    }

public enum Forme
    {
        carre,
        rectangle
    }