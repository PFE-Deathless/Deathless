using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class LootManager : MonoBehaviour
{
    //#### ATTRIBUTS ####
    public List<Objects> lootList = new List<Objects>();
    public GameObject droppedLoot;

    // public void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Item"))
    //     {
    //         if(Input.GetKeyDown(KeyCode.E))
    //         {
                
    //         }
    //     }
    // }

    //DROP ITEM
    Objects GetDroppedItem()
    {
        int randomNumber = Random.Range (1,101); //1-100
        List<Objects>possibleItems = new List<Objects>();
        foreach(Objects item in lootList){
            if(randomNumber <= item.dropWeight)
            {
                possibleItems.Add(item);
                Debug.Log("ItemDropped");
            }
           
        }
        if(possibleItems.Count > 0)
            {
                Objects droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
                Debug.Log(droppedItem.name);
                return droppedItem;
            }
            Debug.Log("No loot dropped");
            return null; 
    }


    //INSTANTIATION DE L'OBJET
    public void instantiateLoot(Vector3 spawnPosition)
    {
        Objects droppedItem = GetDroppedItem();
        if(droppedItem != null)
        {
            //ajout de hauteur pour l'objet
            Vector3 newPosition = new Vector3(0f,0.5f,0f) + spawnPosition;
            GameObject lootGameObject = Instantiate(droppedLoot,newPosition,Quaternion.identity);
            lootGameObject.GetComponent<ItemPickup>().thisLoot = droppedItem;
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.itemSprite; 
        }
    }

}
