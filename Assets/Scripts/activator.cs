using TMPro;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject activable;
    public TextMeshPro tmp;
    private bool inBox = false;

    void Start()
    {
        tmp.enabled = false;
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activable.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 )
        {
            inBox = true;
            tmp.enabled = true;          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            tmp.enabled = false;
            inBox = false;
        }
    }
}
