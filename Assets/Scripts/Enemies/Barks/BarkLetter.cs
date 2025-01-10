using System.Collections;
using TMPro;
using UnityEngine;

public class BarkLetter : MonoBehaviour
{
	TextMeshPro letter;

	public void InitLetter(char letter)
	{
		this.letter = GetComponentInChildren<TextMeshPro>();
		this.letter.text = letter.ToString();
		StartCoroutine(AnimateLetterStart());
	}

	public void MoveLetter()
	{
		StartCoroutine(AnimateLetterEnd());
	}

	IEnumerator AnimateLetterStart()
	{
		Color color = letter.color;
		float elapsedTime = 0f;
		float opacityTime = 0.3f;
		
		letter.color = new Color(color.r, color.g, color.b, 0f);

		while (elapsedTime < opacityTime)
		{
			letter.color = new Color(color.r, color.g, color.b, elapsedTime / opacityTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator AnimateLetterEnd()
	{
		Transform player = GameObject.FindWithTag("Player").transform;
		Vector3 velocity = Vector3.zero;
		Vector3 offset = new Vector3(0f, 1f, 0f);

		while (Vector3.Distance(player.transform.position + offset, transform.position) > 0.2f)
		{
			transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 0.3f);
			yield return null;
		}
	}

}
