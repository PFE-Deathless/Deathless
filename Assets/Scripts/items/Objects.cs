using UnityEngine;

public abstract class Objects : MonoBehaviour
{
    protected bool isPassive; 
    public abstract bool isLootable{get;set;}
    public abstract bool isBuyable{get;set;} 
    public abstract int upgradeCost{get;set;}
    public int stockedSouls;
    public int dropWeight;
    public bool isEnhanced; 
    public GameObject playerChar;
    public abstract bool isEquipped{get;set;}

    void Start(){
        //REFERENCE JOUEUR
        playerChar = GameObject.Find("Player");
        //REFERENCE SCRIPT INVENTAIRE
        
    }

    // ###EFFET BASE & AMELIORE
    public abstract void BaseEffect();
    public abstract void EnhancedEffect();

    // ###MAJ DES AMES & ACTIVATION DE L'AMELIORATION
    public void ChangeSouls(){

        //Modification dans le script inventaire

        //Activation de l'amélioration
        if(stockedSouls > upgradeCost){
            isEnhanced = true; 
        }

    }
}


    


