using UnityEngine;

public class Turret : MonoBehaviour
{
	public Transform objToRotate;
	public float rotationSpeed = 30f;
	public float shootFrequency = 5f;

	float elapsedTime = 0f;
	ProjectileShooter shooter;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		shooter = GetComponent<ProjectileShooter>();
	}

	// Update is called once per frame
	void Update()
	{
		objToRotate.eulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);

		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1f / shootFrequency)
		{
			shooter.ShootProjectile();
			elapsedTime -= 1f / shootFrequency;
		}
	}
}
