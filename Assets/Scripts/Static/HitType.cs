using UnityEngine;

public static class HitType
{
	public enum Type
	{
		A,
		B,
		C,
		None
	}

	public static Type GetRandomType()
	{
		return (Type)Random.Range(0, 3);
	}
}
