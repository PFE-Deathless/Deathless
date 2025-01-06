using UnityEngine;

public static class HitType
{
	static string spriteFolder = "Xbox";

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
				return Resources.Load<Sprite>($"HitSprites/{spriteFolder}/HitA");
			case Type.B:
				return Resources.Load<Sprite>($"HitSprites/{spriteFolder}/HitB");
			case Type.C:
				return Resources.Load<Sprite>($"HitSprites/{spriteFolder}/HitC");
			default:
				return null;
		}
	}
}
