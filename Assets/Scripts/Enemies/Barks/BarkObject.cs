using System.Collections;
using UnityEngine;

public class BarkObject : MonoBehaviour
{
	[SerializeField, Tooltip("Letter size")] float size = 0.5f;
	[SerializeField, Tooltip("Offset between letters")] float offset = 0.4f;
	[SerializeField, Tooltip("Duration of the spawning time")] float startTime = 0.3f;
	[SerializeField, Tooltip("Movement the letters do upward while spawning")] float movement = 3f;
	[SerializeField, Tooltip("Time waited at full size before next sequence")] float waitTime = 1f;
	[SerializeField, Tooltip("Time it takes for the letter to scale down and shards to scale up")] float endTime = 0.1f;
	[SerializeField, Tooltip("Acceleration for each letter once they start moving")] float acceleration = 5f;

	BarkLetter[] barkLetters;

	public void InitBark(int size)
	{
		string bark = EnemyBarks.GetRandomBark(size);
		GameObject letterObj = Resources.Load<GameObject>("Barks/BarkLetter");
		barkLetters = new BarkLetter[size];
		for (int i = 0; i < size; i++)
		{
			Vector3 newPos = transform.position + new Vector3(((this.size + offset) * i) - ((this.size + offset) * (size - 1)) / 2f, 0f, 0f);
			GameObject obj = Instantiate(letterObj, newPos, Quaternion.identity, transform);
			barkLetters[i] = obj.GetComponent<BarkLetter>();
			barkLetters[i].InitLetter(bark[i]);
		}
		StartCoroutine(AnimateText());
	}

	IEnumerator AnimateText()
	{
		float elapsedTime = 0f;
		float movementPerSecond = movement / startTime;


		// Full word scale up while moving upward, and letter opacity goes from 0 to 1
		while (elapsedTime < startTime)
		{
			transform.localScale = new Vector3(elapsedTime / startTime, elapsedTime / startTime, elapsedTime / startTime);
			transform.position += new Vector3(0f, movementPerSecond * Time.deltaTime, 0f);
			foreach (var letter in barkLetters)
			{
				letter.SetOpacity(elapsedTime / startTime);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Wait with the full word at normal scale so the player can read it
		yield return new WaitForSeconds(waitTime);

		// Scale down the letter while scaling up the soul shards
		elapsedTime = 0f;
		while (elapsedTime < endTime)
		{
			foreach (var letter in barkLetters)
			{
				letter.SetShardScale(elapsedTime / endTime);
				letter.SetLetterScale(1f - (elapsedTime / endTime));
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Find player and make the letters move towards it
		Transform target = GameObject.FindWithTag("Player").transform;
		foreach (var letter in barkLetters)
		{
			letter.MoveLetter(target, acceleration);
		}
	}
}
