using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
	List<Enemy> _enemiesInside = new List<Enemy>();
	HitType.Type type;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 7)
		{
			Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
			if (!_enemiesInside.Contains(enemy))
			{
				if (enemy.CurrentType == type)
					enemy.TakeDamage();

				_enemiesInside.Add(enemy);
			}
		}
	}

	public void SetType(HitType.Type type)
	{
		this.type = type;
		GetComponentInChildren<SpriteRenderer>().sprite = HitType.GetSprite(type);
	}

	private void OnDisable()
	{
		_enemiesInside.Clear();
	}
}
