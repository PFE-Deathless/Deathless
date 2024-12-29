using UnityEngine;
using UnityEngine.UI;

public class SoulsDisplay : MonoBehaviour
{
    public Text soulsText;

    public void UpdateSouls (int value)
    {
        soulsText.text = value.ToString ();
    }
}
