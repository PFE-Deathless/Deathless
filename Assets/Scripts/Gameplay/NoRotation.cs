using UnityEngine;

public class NoRotation : MonoBehaviour
{
	[SerializeField] Type type;

	void LateUpdate()
	{
		switch (type)
		{
			case Type.None:
				transform.rotation = Quaternion.identity;
				break;
			case Type.Local:
				transform.localRotation = Quaternion.identity;
				break;
			case Type.UpAxis:
				transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
				break;
		}
	}

	enum Type
	{
		None,
		Local,
		UpAxis,
	}
}
