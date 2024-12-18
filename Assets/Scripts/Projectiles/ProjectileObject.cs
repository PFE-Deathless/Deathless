using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/Projectile")]
public class ProjectileObject : ScriptableObject
{
	public GameObject gameObject;
	public float speed;
	public float lifeSpan = 10f;
	public int damage = 1;
	public bool destroyOnContact = true;

}
