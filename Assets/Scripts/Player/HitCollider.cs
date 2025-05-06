using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
	List<Enemy> _enemiesInside = new List<Enemy>();
	List<Dummy> _dummiesInside = new List<Dummy>();
	HitType.Type type;

	bool _hitSuccess = false;

	public bool HitSucess => _hitSuccess;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10) // Interactable
		{
			other.gameObject.GetComponent<IInteractable>().Interact(InteractableType.Hit);
		}

		if (other.gameObject.layer == 7) // Enemy
		{
			Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
			if (!_enemiesInside.Contains(enemy))
			{
				if (enemy.CurrentType == type)
				{
					enemy.TakeDamage();
					_hitSuccess = true;
				}

				_enemiesInside.Add(enemy);
			}
		}

		if (other.gameObject.layer == 11) // Dummy
		{
			Dummy dummy = other.gameObject.GetComponentInParent<Dummy>();
			if (!_dummiesInside.Contains(dummy))
			{
				if (dummy.CurrentType == type)
				{
					dummy.TakeDamage();
					_hitSuccess = true;

				}

				_dummiesInside.Add(dummy);
			}
		}
	}

	public void SetType(HitType.Type type)
	{
		this.type = type;
	}

	private void OnDisable()
	{
		_hitSuccess = false;
		_enemiesInside.Clear();
		_dummiesInside.Clear();
	}
}
