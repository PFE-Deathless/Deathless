using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    [Header("UI a activer/d�sactiver")]
    public int id;
    [Header("Mode")]
    public bool activate;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 6)
        {
            TutoManager.Instance.Activate(id, activate);
            Destroy(gameObject);
        }
    }


    /*// R�f�rences
    [Header("UI a activer/d�sactiver")]
    public GameObject infoUI;
    [Header("Mode")]
    public bool activate;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("i");
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 6)
        {
            if (activate)
            {
                infoUI.SetActive(true);
                Destroy(gameObject);
            }
            if (!activate)
            {
                infoUI.SetActive(false);
                Destroy(gameObject);
            }
        }
    }*/
}
