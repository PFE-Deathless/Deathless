using UnityEngine;

public class TestItem1_scripting : Objects
{
    [SerializeField]
    public Objects scriptableReference;

    public override void SoulsGained()
    {
        if (isEquipped)
        {
            if (isEnhanced == true)
            {
                EnhanceEffect();
            }
            else
            {
                ActiveEffect();
            }
        }
        else
        {
            Debug.Log(itemName + " is not equipped");
        }
    }

    public override void ActiveEffect()
    {
        Debug.Log(itemName + " is active");
    }

    public override void EnhanceEffect()
    {
        Debug.Log(itemName + " is enhanced");
    }
}
