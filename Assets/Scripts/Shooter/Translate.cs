using UnityEngine;

public class Translate : Move
{
	[SerializeField] AnimationCurve movementCurve;
	[SerializeField] float movementSpeed = 5f;
	[SerializeField] Transform start;
	[SerializeField] Transform end;

	bool direction = true;
	float distance;
	float time;
	float t;

	protected override void Setup()
	{
		base.Setup();
		distance = Vector3.Distance(start.position, end.position);
		time = distance / movementSpeed;
	}

	protected override void PerformMove()
	{
		//Debug.Log("test");
		if (direction)
		{
			t += Time.fixedDeltaTime;
			if (t >= time)
			{
				direction = false;
				t = time;
			}
		}
		else
		{
			t -= Time.fixedDeltaTime;
			if (t <= 0f) 
			{ 
				direction = true;
				t = 0f;
			}
		}
		
		target.position = Vector3.Lerp(start.position, end.position, movementCurve.Evaluate(t / time));
	}
}
