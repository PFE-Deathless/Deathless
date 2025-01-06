using UnityEngine;

public class PingPong : Move 
{
	[SerializeField] AnimationCurve movementCurve;
	[SerializeField] float rotationSpeed = 30f;
	[SerializeField] float angleOffset = 0f;
	[SerializeField] float angleRange = 120f;

	bool direction = true;
	float angle;
	float t;

	protected override void PerformMove()
	{
		//Debug.Log("test");
		if (direction)
		{
			t += Time.fixedDeltaTime * rotationSpeed;
			if (t >= angleRange)
			{
				direction = false;
				t = angleRange;
			}
		}
		else
		{
			t -= Time.fixedDeltaTime * rotationSpeed;
			if (t <= 0f)
			{
				direction = true;
				t = 0f;
			}
		}

		angle = (movementCurve.Evaluate(t / angleRange) * angleRange) + angleOffset;

		target.eulerAngles = new Vector3(target.eulerAngles.x, angle, target.eulerAngles.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Sin((angleOffset) * Mathf.Deg2Rad), 0f, Mathf.Cos((angleOffset) * Mathf.Deg2Rad)) * 10f);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Sin((angleOffset + angleRange) * Mathf.Deg2Rad), 0f, Mathf.Cos((angleOffset + angleRange) * Mathf.Deg2Rad)) * 10f);
	}
}
