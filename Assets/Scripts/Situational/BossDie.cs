using UnityEngine;

public class BossDie : MonoBehaviour
{
    [SerializeField] GameObject boss;

    void Update()
    {
        if (boss == null) gameObject.GetComponentInChildren<Door>().Activate();
        
    }
}
