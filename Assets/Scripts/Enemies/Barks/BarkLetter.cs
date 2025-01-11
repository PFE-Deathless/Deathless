using System.Collections;
using TMPro;
using UnityEngine;

public class BarkLetter : MonoBehaviour
{
	[SerializeField] TextMeshPro letter;
	[SerializeField] GameObject soulShard;

	Color color;
	float currentSpeed = 0f;
	float acceleration;
	Transform target;

	public void InitLetter(char letter)
	{
		this.letter.text = letter.ToString();
		color = this.letter.color;
		SetShardScale(0f);
	}

	public void SetOpacity(float opacity)
	{
		letter.color = new Color(color.r, color.g, color.b, opacity);
	}

	public void SetShardScale(float scale)
	{
		soulShard.transform.localScale = new Vector3(scale, scale, scale);
	}

	public void SetLetterScale(float scale)
	{
		letter.transform.localScale = new Vector3(scale, scale, scale);
	}

	public void MoveLetter(Transform target, float acceleration)
	{
		this.target = target;
		this.acceleration = acceleration * Random.Range(0.8f, 1.2f);
		GetComponentInChildren<TrailRenderer>().emitting = true;
		StartCoroutine(AnimateMoveLetter());
	}

	IEnumerator AnimateMoveLetter()
	{
		Vector3 direction;
		Vector3 offset = new(0f, 1f, 0f);

		while (Vector3.Distance(target.transform.position + offset, transform.position) > 0.2f)
		{
			direction = (target.transform.position + offset - transform.position).normalized;
			currentSpeed += Time.deltaTime * acceleration;
			transform.position += currentSpeed * Time.deltaTime * direction;
			yield return null;
		}

		target.gameObject.GetComponent<PlayerSouls>().AddSouls(1);
		Destroy(gameObject);
	}
}
