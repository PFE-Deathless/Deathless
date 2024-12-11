using UnityEngine;

public class Rotator : MonoBehaviour
{
	public Transform objToRotate;
	public float rotationSpeed = 30f;

	void Update()
	{
		objToRotate.eulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
