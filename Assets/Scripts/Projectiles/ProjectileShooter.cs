using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	public ProjectileObject projectile;
	public Transform origin;

	public void ShootProjectile()
	{
		GameObject obj = Instantiate(projectile.gameObject, origin.position, origin.rotation);
		obj.AddComponent<Rigidbody>();
		Projectile p = obj.AddComponent<Projectile>();
		p.Setup(projectile.speed, null);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
		{
			ShootProjectile();
		}
    }
}
