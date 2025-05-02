using System;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerQuitGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
        {
            Application.Quit();
        }
    }
}
