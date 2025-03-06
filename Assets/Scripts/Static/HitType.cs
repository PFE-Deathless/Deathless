using UnityEngine;

public static class HitType
{
	static string spriteFolder = "Xbox";

	public enum Type
	{
		A,
		B,
		C,
		None
	}

	public enum Controller
	{
		Keyboard,
		Xbox,
		Playstation
	}

	public static Type GetRandomType()
	{
		return (Type)Random.Range(1, 4);
	}

	public static void SetController(Controller controller)
	{
		switch (controller)
		{
			case Controller.Keyboard:
				spriteFolder = "Keyboard";
				break;
			case Controller.Xbox:
				spriteFolder = "Xbox";
				break;
			case Controller.Playstation:
				spriteFolder = "Playstation";
				break;
			default:
				break;
		}
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
