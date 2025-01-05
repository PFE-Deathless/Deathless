using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemList{
    public Item item;
    public string name;

    public ItemList(Item newItem, string newName){
        item = newItem;
        name = newName;
    }
}
