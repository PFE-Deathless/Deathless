using UnityEngine;

public static class HitType
{
	static string spriteFolder = "Universal";

	public enum Type
	{
		A,
		B,
		C,
		None
	}

	public enum Controller
	{
		Universal,
		Keyboard,
		Xbox,
		Playstation
	}

	public static Type GetRandomType()
	{
		return (Type)Random.Range(0, 3);
	}

	public static void SetController(Controller controller)
	{
		switch (controller)
		{
            case Controller.Universal:
                spriteFolder = "Universal";
                break;
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
