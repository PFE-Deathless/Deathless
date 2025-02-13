using UnityEngine;

public class ForceRotation : MonoBehaviour
{
	public bool local;
	public Quaternion rotation;

	void Update()
	{
		if (local)
			transform.localRotation = rotation;
		else
			transform.rotation = rotation;
	}
}
