using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
	List<Enemy> _enemiesInside = new List<Enemy>();
	List<Dummy> _dummiesInside = new List<Dummy>();
	HitType.Type type;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 7) // Enemy
		{
			Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
			if (!_enemiesInside.Contains(enemy))
			{
				if (enemy.CurrentType == type)
					enemy.TakeDamage();

				_enemiesInside.Add(enemy);
			}
		}

		if (other.gameObject.layer == 11) // Dummy
		{
			Dummy dummy = other.gameObject.GetComponentInParent<Dummy>();
			if (!_dummiesInside.Contains(dummy))
			{
				if (dummy.CurrentType == type)
					dummy.TakeDamage();

				_dummiesInside.Add(dummy);
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
		_dummiesInside.Clear();
	}
}
