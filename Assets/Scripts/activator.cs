using TMPro;
using UnityEngine;

public class activator : MonoBehaviour
{
    public GameObject activable;
    public TextMeshPro tmp;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 )
        {
            tmp
            
        }
    }
}
