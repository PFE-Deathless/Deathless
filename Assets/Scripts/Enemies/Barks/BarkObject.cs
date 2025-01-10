using System.Collections;
using UnityEngine;

public class BarkObject : MonoBehaviour
{
	float size = 0.5f;
	float offset = 0.4f;

	BarkLetter[] barkLetters;
	

	public void InitBark(int size)
	{
		string bark = EnemyBarks.GetRandomBark(size);
		barkLetters = new BarkLetter[size];
		for (int i = 0; i < size; i++)
		{
			Vector3 newPos = transform.position + new Vector3(((this.size + offset) * i) - ((this.size + offset) * (size - 1)) / 2f, 0f, 0f);
			GameObject obj = Instantiate(Resources.Load<GameObject>("Barks/BarkLetter"), newPos, Quaternion.identity, transform);
			barkLetters[i] = obj.GetComponent<BarkLetter>();
			barkLetters[i].InitLetter(bark[i]);
		}
		StartCoroutine(AnimateText());
	}

	IEnumerator AnimateText()
	{
		float elapsedTime = 0f;
		float startTime = 0.3f;
		float movementPerSecond = 10f;

		while (elapsedTime < startTime)
		{
			transform.localScale = new Vector3(elapsedTime / startTime, elapsedTime / startTime, elapsedTime / startTime);
			transform.position += new Vector3(0f, movementPerSecond * Time.deltaTime, 0f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(2f);

		for (int i = 0; i < barkLetters.Length; i++)
		{
			barkLetters[i].MoveLetter();
		}
	}
}
