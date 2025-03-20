using UnityEngine;

public class NewHitBar : MonoBehaviour
{
	[Header("Sprites")]
	[SerializeField] Sprite backgroundSprite;
	[SerializeField] Sprite hitACurrentSprite;
	[SerializeField] Sprite hitANextSprite;
	[SerializeField] Sprite hitBCurrentSprite;
	[SerializeField] Sprite hitBNextSprite;
	[SerializeField] Sprite hitCCurrentSprite;
	[SerializeField] Sprite hitCNextSprite;

	[Header("Sprite Renderers")]
	[SerializeField] SpriteRenderer backgroundSR;
	[SerializeField] SpriteRenderer currentSR;
	[SerializeField] SpriteRenderer nextSR;

	private void Start()
	{
		backgroundSR.sprite = backgroundSprite;
	}

	public void SetTypes(HitType.Type[] types, int index)
	{
		SetTypes(types[index], types.Length > index + 1 ? types[index + 1] : HitType.Type.None);
	}

	public void SetTypes(HitType.Type current, HitType.Type next)
	{
		switch (current)
		{
			case HitType.Type.A:
				currentSR.sprite = hitACurrentSprite;
				break;
			case HitType.Type.B:
				currentSR.sprite = hitBCurrentSprite;
				break;
			case HitType.Type.C:
				currentSR.sprite = hitCCurrentSprite;
				break;
			default:
				currentSR.sprite = null;
				break;
		}

		switch (next)
		{
			case HitType.Type.A:
				nextSR.sprite = hitANextSprite;
				break;
			case HitType.Type.B:
				nextSR.sprite = hitBNextSprite;
				break;
			case HitType.Type.C:
				nextSR.sprite = hitCNextSprite;
				break;
			default:
				nextSR.sprite = null;
				break;
		}
	}
}
