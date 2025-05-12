using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
	[SerializeField, Tooltip("Layers the hit cannot traverse")] LayerMask wallLayerMask;

	readonly List<Enemy> _enemiesInside = new();
	readonly List<Dummy> _dummiesInside = new();
	HitType.Type type;

	bool _hitSuccess = false;

	public bool HitSucess => _hitSuccess;

	private void OnTriggerEnter(Collider other)
	{
		// Check if a wall is hit before applying damage
		Vector3 origin = transform.position;
		Vector3 destination = other.transform.position;

		origin.y = 0.5f;
		destination.y = 0.5f;

		Vector3 direction = (destination - origin).normalized;
		Ray ray = new(origin, direction);
		if (Physics.Raycast(ray, Vector3.Distance(transform.position, other.transform.position), wallLayerMask))
			return;

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
