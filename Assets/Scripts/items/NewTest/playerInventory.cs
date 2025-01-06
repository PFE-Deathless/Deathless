using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerInventory : MonoBehaviour
{
    //référence
    public List<ItemList>items = new List<ItemList>();

    void Start(){
        //Test ajout d'un item manuellement  (fonctionne)
        HealingItem item = new HealingItem();
        items.Add(new ItemList(item, item.GiveName()));
        //Update pour activer les items, sans laguer avec une Update si on a 50k items complexes (permet d'update les valeurs des items)
        StartCoroutine(CallItemUpdate());
    }
    

    IEnumerator CallItemUpdate(){
        foreach(ItemList i in items){
            //ici les références aux scripts joueur qu'on voudrait ptêt modifier (si on veux ajouter un truc on le rajoute dans le Update de l'item.)
            i.item.Update(GetComponent<PlayerController>(), GetComponent<PlayerHealth>());
        }

        //delai pour le lag
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    
}
