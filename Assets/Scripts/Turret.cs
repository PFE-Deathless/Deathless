using UnityEngine;

public class Turret : LoopShooter
{
	public Transform objToRotate;
	public float rotationSpeed = 30f;

	new void Update()
	{
		base.Update();
		objToRotate.eulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
