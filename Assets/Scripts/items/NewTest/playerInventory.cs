using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerInventory : MonoBehaviour
{
    public List<ItemList>items = new List<ItemList>();

    void Start(){
        HealingItem item = new HealingItem();
        items.Add(new ItemList(item, item.GiveName()));
        StartCoroutine(CallItemUpdate());
    }
    

    IEnumerator CallItemUpdate(){
        foreach(ItemList i in items){
            i.item.Update(GetComponent<PlayerController>(), GetComponent<PlayerHealth>());
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    
}
