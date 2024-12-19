using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Scripting;

[CreateAssetMenu]
public class Objects : ScriptableObject
{
    //#### ATTRIBUTS ####
    protected bool isPassive; 
    public bool isBuyable; 
    public int upgradeCost;
    public int stockedSouls;
    public int dropWeight;
    public bool isEnhanced; 
    public GameObject playerChar;
    public Inventory  inventoryScript;
    public bool isEquipped;
    public string itemName;
    public string scriptName;
    public Sprite itemSprite;
    public MeshFilter itemMesh;
    public MeshRenderer itemRenderer;
    public GameObject abilityPrefab;
    void Start(){
        //REFERENCE JOUEUR
        playerChar = GameObject.Find("Player");
        //REFERENCE SCRIPT INVENTAIRE
        inventoryScript  = playerChar.GetComponent<Inventory>();
        itemName = this.name;
    }

    // ###EFFET BASE & AMELIORE
    // public abstract void BaseEffect();
    // public abstract void EnhancedEffect();

    // ###MAJ DES AMES & ACTIVATION DE L'AMELIORATION
    public void Update(){
        //Modification dans le script inventaire
        stockedSouls = Inventory.souls;
        //Activation de l'amélioration
        if(stockedSouls > upgradeCost)
        {
            isEnhanced = true; 
        }
    }
    public virtual void SoulsGained() {}
    public virtual void ActiveEffect(){}
    public virtual void EnhanceEffect(){}
}


    


