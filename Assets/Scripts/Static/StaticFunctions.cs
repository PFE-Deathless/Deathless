using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFunctions
{
	public static Transform GetNearest(List<Transform> list, Vector3 position)
	{
		if (list == null)
			return null;

		if (list.Count == 1)
			return list[0];

		float nearestDistance = Mathf.Infinity;
		float distance;
		Transform nearest = null;

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null) continue;
			distance = Vector3.Distance(list[i].position, position);
			if (distance < nearestDistance)
			{
				nearest = list[i];
				nearestDistance = distance;
			}
		}

		return nearest;
	}

	public static Transform GetNearest(Transform[] array, Vector3 position)
	{
		if (array == null)
			return null;

		if (array.Length == 1)
			return array[0];

		float nearestDistance = Mathf.Infinity;
		float distance;
		Transform nearest = null;

		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null) continue;
			distance = Vector3.Distance(array[i].position, position);
			if (distance < nearestDistance)
			{
				nearest = array[i];
				nearestDistance = distance;
			}
		}

		return nearest;
	}
}