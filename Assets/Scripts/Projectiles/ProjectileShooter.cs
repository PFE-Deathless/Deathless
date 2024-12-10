using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	public ProjectileObject projectile;
	public Transform origin;

	public void ShootProjectile()
	{
		GameObject obj = Instantiate(projectile.gameObject, origin.position, origin.rotation);
		obj.AddComponent<Rigidbody>();
		ProjectileMovement p = obj.AddComponent<ProjectileMovement>();
		p.Setup(projectile);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			ShootProjectile();
		}
	}
}
