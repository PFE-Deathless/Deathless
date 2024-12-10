using UnityEngine;

public class NoRotation : MonoBehaviour
{
	void Update()
	{
		transform.rotation = Quaternion.identity;
	}
}
