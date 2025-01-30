using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
	// Properties
	[SerializeField] private int id;
	[SerializeField] private string itemName = "DEFAULT_NAME";
	[SerializeField] private Texture2D icon;
	[SerializeField] private string description = "DEFAULT_DESCRIPTION";
	[SerializeField] private string flavorText = "DEFAULT_FLAVOR";

	// Attributes
	public int ID { get { return id; } }
	public string Name { get { return itemName; } }
	public string Description { get { return description; } }
	public string FlavorText { get { return flavorText; } }
}
