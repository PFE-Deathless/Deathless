using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NewLootManager : MonoBehaviour
{
    //#### ATTRIBUTS ####
    public List<GameObject> lootList = new List<GameObject>();
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
    GameObject GetDroppedItem()
    {
        int randomNumber = Random.Range (1,101); //1-100
        List<GameObject>possibleItems = new List<GameObject>();
        foreach(GameObject item in lootList){
            Pickup itemPickup = item.GetComponent<Pickup>();
            Debug.Log(itemPickup.dropWeight);
            if(randomNumber <= itemPickup.dropWeight)
            {
                possibleItems.Add(item);
                Debug.Log("ItemDropped");
            }
           
        }
        if(possibleItems.Count > 0)
            {
                GameObject droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
                Debug.Log(droppedItem.name);
                return droppedItem;
            }
            Debug.Log("No loot dropped");
            return null; 
    }


    //INSTANTIATION DE L'OBJET
    public void instantiateLoot(Vector3 spawnPosition)
    {
        GameObject droppedItem = GetDroppedItem();
        if(droppedItem != null)
        {
            //ajout de hauteur pour l'objet
            Vector3 newPosition = new Vector3(0f,0.5f,0f) + spawnPosition;
            GameObject lootGameObject = Instantiate(droppedItem,newPosition,Quaternion.identity); 
        }
    }

}
