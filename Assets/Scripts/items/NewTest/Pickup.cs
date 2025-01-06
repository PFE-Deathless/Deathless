using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item  item;
    public Items itemDrop;
    public GameObject playerChar;
    //valeur par défaut au cas ou
    public int dropWeight = 50;
    void Start()
    {
       
        //REFERENCE PLAYER
        playerChar = GameObject.Find("Player");
    }

    public void AddItem(playerInventory player)
    {
        //check si item avec le  même nom  est déjà présent
        foreach(ItemList i in player.items){
            if(i.name == item.GiveName()){
                //si oui ne retourne rien
                return;
            }
        }
        //Si item n'est pas déjà présent ajoute l'item (ça  ne marche  pas ;-;)
        player.items.Add(new ItemList(item, item.GiveName()));
    }

    //référence atous les items qu'on peux assigner avec un switch (modifiable dans l'éditeur)
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

       //j'ai tenté avec l'input sa ne fonctionnait pas donc j'ai  juste mis la  hitbox pour  le moment
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
