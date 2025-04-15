using UnityEngine;

public class Rotator : Move
{
	[SerializeField] float rotationSpeed = 30f;

	protected override void PerformMove()
	{
		target.eulerAngles += new Vector3(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
	}
}
