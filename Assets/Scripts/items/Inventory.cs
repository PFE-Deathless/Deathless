using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Objects))]
public class Inventory : MonoBehaviour
{
	//### ATTRIBUTS ###
	public static int souls;
	public bool hasActiveItem;
	public int maxPassiveItem;
	public bool isInventoryActive;
	public List<Objects> inventory = new List<Objects>();


	//Reset des valeurs au start
	public void Start()
	{
		hasActiveItem = false;
		souls  = 0; 
		maxPassiveItem = 5;
	}


	//Ajout d'un item a l'inventaire
	public void AddItem(Objects item){
		if (inventory.Count <= maxPassiveItem)
		inventory.Add(item);
	}
}
