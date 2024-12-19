using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/Projectile")]
public class ProjectileObject : ScriptableObject
{
	public GameObject gameObject;
	[Tooltip("Will activate on projectile destroy, keep it null for no effect")] public EffectObject effect;
	[Tooltip("GameObject that make the previsualisation of the object (curve only)")] public GameObject previsualisation;
	public float speed;
	public float lifeSpan = 10f;
	public int damage = 1;
	public bool destroyOnContact = true;
	public Type type = Type.Forward;

	public enum Type
	{
		Forward,
		Homing,
		Curve
	}
}
