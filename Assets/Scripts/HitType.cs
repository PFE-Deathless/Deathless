using UnityEngine;

public static class HitType
{
	public enum Type
	{
		None,
		A,
		B,
		C
	}

	public static Type GetRandomType()
	{
		return (Type)Random.Range(1, 4);
	}

	public static Sprite GetSprite(Type type)
	{
		switch (type)
		{
			case Type.A:
				return Resources.Load<Sprite>("HitA");
			case Type.B:
				return Resources.Load<Sprite>("HitB");
			case Type.C:
				return Resources.Load<Sprite>("HitC");
			default:
				return null;
		}
	}
}
