using UnityEngine;

[CreateAssetMenu(fileName = "EffectObject", menuName = "Scriptable Objects/Effect")]
public class EffectObject : ScriptableObject
{
    public GameObject gameObject;
    public float duration = 1f;
    public float radius = 2f;
    public int damage = 1;
}
