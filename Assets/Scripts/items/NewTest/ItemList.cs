using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ce  sript sert a donner la  forme des listes  d'items qu'on veux partout dans les autres scripts. 
[System.Serializable]
public class ItemList{
    public Item item;
    public string name;

    public ItemList(Item newItem, string newName){
        item = newItem;
        name = newName;
    }
}
