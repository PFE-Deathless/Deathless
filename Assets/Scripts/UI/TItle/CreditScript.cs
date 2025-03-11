using UnityEngine;

public class CreditScript : MonoBehaviour
{
    public GameObject canvaActive;
    public GameObject canvaDeactivate;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void LoadCanva()
    {
        canvaActive.SetActive(true);
        canvaDeactivate.SetActive(false);
    }

}
