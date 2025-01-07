using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public TMP_Text interactText;
    public Objects  thisLoot;
    public GameObject playerChar;
    private bool playerInside;


    void Start()
    {
        //REFERENCE TEXTE UI
        interactText = GetComponentInChildren<TMP_Text>();
        //DESACTIVATION TEXT UI
        interactText.enabled = false;
        //REFERENCE PLAYER
        playerChar = GameObject.Find("Player");
        //Joueure dedans?
        playerInside = false;
    }

    //Detection si joueur rentre
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " collided!");
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interactText.enabled = true;
            playerInside = true;
            Debug.Log(playerInside);
        }
    }

    //Ramasser l'item
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & playerInside == true)
        {
            playerChar.GetComponent<Inventory>().AddItem(thisLoot);
            Debug.Log("Added " + thisLoot.name + " to iventory");
            Destroy(gameObject);
        }
    }

    //Detection si joueur sort
    void OnTriggerExit(Collider other){
        Debug.Log(other + " has left");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interactText.enabled = false;
            playerInside = false;
            Debug.Log(playerInside);
        }
    }
}
