using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item  item;
    public Items itemDrop;
    public GameObject playerChar;
    public int dropWeight = 50;
    void Start()
    {
        //REFERENCE TEXTE UI
        //interactText = GetComponentInChildren<TMP_Text>();
        //DESACTIVATION TEXT UI
        //interactText.enabled = false;
        //REFERENCE PLAYER
        playerChar = GameObject.Find("Player");
        //Joueur dedans?
    }

    public void AddItem(playerInventory player)
    {
        foreach(ItemList i in player.items){
            if(i.name == item.GiveName()){
                return;
            }
        }
        player.items.Add(new ItemList(item, item.GiveName()));
    }

    public Item AssignItem(Items itemToAssign)
    {
        switch(itemToAssign)
        {
            case Items.HealingItem:
                return new HealingItem();
            case  Items.TestItemOne:
                return new TestItemOne();
            case Items.TestActiveItem:
                return new TestActiveItem();
            default:
                return new HealingItem();
    

        }
    }

       public void OnTriggerStay(Collider other)
    {
        Debug.Log(other + " collided!");
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInventory player = other.GetComponent<playerInventory>();
            Debug.Log(other + " Player!");
            AddItem(player);
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     AddItem(player);
            //     Destroy(gameObject);
            // }
        }
    }

}



public enum Items
{
        HealingItem,        
        TestItemOne,
        TestActiveItem
    }
