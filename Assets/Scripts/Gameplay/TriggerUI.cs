using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    // Références
    [Header("UI a activer/désactiver")]
    public GameObject infoUI;
    [Header("Mode")]
    public bool activate;

    bool done = false;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("i");
        if (collider.gameObject.layer == 3)
        {
            if (!done && activate)
            {
                infoUI.SetActive(true);
                done = true;
            }
            if (!done && !activate)
            {
                infoUI.SetActive(false);
                done = true;
            }
        }
    }
}
