using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		//gameObject.transform.eulerAngles = new Vector3(-Quaternion.LookRotation(Camera.main.transform.position, Vector3.forward).eulerAngles.x, 0f, 0f);
		if (Camera.main != null)
			gameObject.transform.rotation = Camera.main.transform.rotation;
	}
}
