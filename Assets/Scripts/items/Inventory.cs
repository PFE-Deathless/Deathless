using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(Objects))]
public class Inventory : MonoBehaviour
{
	//### ATTRIBUTS ###
	public static int souls;
	public bool hasActiveItem;
	public int maxPassiveItem;
	public bool isInventoryActive;
	public List<Objects> inventory = new List<Objects>();
	public GameObject lootManager;


	//Reset des valeurs au start
	public void Start()
	{
		hasActiveItem = false;
		souls  = 0; 
		maxPassiveItem = 5;
		lootManager = GameObject.Find("Loot Manager");
	}


	//Ajout d'un item a l'inventaire
	public void AddItem(Objects item){
		if (inventory.Count <= maxPassiveItem)
		{
			inventory.Add(item);
			item.isEquipped = true;
			if (item.itemID == 0)
        	{
			}
			else 
			{
				if (item.itemID == 1)
				{

				}
			}
		} 
		else
		{
			//Si trop d'item
			Debug.Log("Trop d'items!");
		}
	}


	//Ajout d'ames
	public void GainSouls(int soulsGain)
	{
		souls =+ soulsGain;
		//Verification des objets dans l'inventaire
		foreach (Objects item in inventory) 
		{ 
				item.SoulsGained();
		}
	}

}
