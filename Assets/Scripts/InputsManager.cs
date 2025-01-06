using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
	[HideInInspector] public Vector2 move;
	[HideInInspector] public HitType.Type hit;
	[HideInInspector] public bool dash;
	[HideInInspector] public bool reloadScene;

	public void OnMove(InputValue value)
	{
		move = value.Get<Vector2>().normalized;
	}

	public void OnHitA()
	{
		hit = HitType.Type.A;
	}

	public void OnHitB()
	{
		hit = HitType.Type.B;
	}

	public void OnHitC()
	{
		hit = HitType.Type.C;
	}

	public void OnDash()
	{
		dash = true;
	}

	public void OnReloadScene()
	{
		reloadScene = true;
	}
}
