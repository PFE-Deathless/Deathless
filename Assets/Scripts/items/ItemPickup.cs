using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public TMP_Text interactText;

    void Start()
    {
        //REFERENCE TEXTE UI
        interactText = GetComponentInChildren<TMP_Text>();
        //DESACTIVATION TEXT UI
        interactText.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            interactText.enabled = true;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            interactText.enabled = false;
        }
    }
}
