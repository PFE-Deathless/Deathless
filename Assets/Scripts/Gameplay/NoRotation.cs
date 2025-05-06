using UnityEngine;

public class NoRotation : MonoBehaviour
{
	public bool local;

	void Update()
	{
		if (local)
			transform.localRotation = Quaternion.identity;
		else
			transform.rotation = Quaternion.identity;
	}
}
