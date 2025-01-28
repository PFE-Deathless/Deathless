using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerate : MonoBehaviour
{
    public List<Salle> allSalles = new List<Salle>();
    public List<Salle> genSalle = new List<Salle>();

    private List<Salle> canSalle = new List<Salle>();



    void Start()
    {       
        ManageLoad(); 
    }

    void ManageLoad()
    {
        foreach (Salle S in genSalle)
        {
            TriSalle(S);

            int i = canSalle.Count;
            print("i = "+i);
            int j = Random.Range(0, i);
            print("j = " + j);
            Instantiate(canSalle[j].gameObject, S.gameObject.transform.position, S.gameObject.transform.rotation);
            canSalle.Clear();
            Destroy(S.gameObject);

        }
 
    }


    void TriSalle(Salle S)
    {
        foreach (Salle s in allSalles)
        {
            bool can = true;

            if (s.fCarre == S.fCarre) can = true;
            else can = false;
            if (s.fRectangle == S.fRectangle) can = true;
            else can = false;


            if (s.tEnnemis == S.tEnnemis) can = true;
            else can = false;
            if (s.tTourelles == S.tTourelles) can = true;
            else can = false;
            if (s.tPieges == S.tPieges) can = true;
            else can = false;

            if (can == true) canSalle.Add(s);
        }
    }

}
