using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/Projectile")]
public class ProjectileObject : ScriptableObject
{
	public GameObject gameObject;
	public float speed;
	public bool homing;

}
