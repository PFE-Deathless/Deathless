using UnityEngine;

public class Salle : MonoBehaviour
{
    [Header("Forme")]
    public bool fCarre;
    public bool fRectangle;

    [Header("Type")]
   
    public bool tEnnemis;
    public bool tTourelles;
    public bool tPieges;



    


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    
}

    public enum tEnnemis 
    { 
        Inclusif,
        Exclusif,
        Bonus
    }
