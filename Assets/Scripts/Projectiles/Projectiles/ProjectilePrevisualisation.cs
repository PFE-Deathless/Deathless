using UnityEngine;

public class ProjectilePrevisualisation : MonoBehaviour
{
	[SerializeField] GameObject circleIn;
	[SerializeField] GameObject circleOut;
	float radius = 1f;

	public void Setup(float radius)
	{
		this.radius = radius * 2f;
		circleOut.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
		circleIn.transform.localScale = new Vector3(0f, 0f, 0f);
	}

	public void SetSize(float t)
	{
		circleIn.transform.localScale = new Vector3(radius * t, radius * t, radius * t);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}
