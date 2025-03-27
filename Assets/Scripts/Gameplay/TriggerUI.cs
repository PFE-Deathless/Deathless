using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    // R�f�rences
    [Header("UI a activer/d�sactiver")]
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
