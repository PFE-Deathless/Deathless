using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
	[HideInInspector] public Vector2 move;
	[HideInInspector] public HitType.Type hit;

	public void OnMove(InputValue value)
	{
		move = value.Get<Vector2>();
	}

	public void OnHitA(InputValue value)
	{
		hit = value.isPressed ? HitType.Type.A : HitType.Type.None;
	}

	public void OnHitB(InputValue value)
	{
		hit = value.isPressed ? HitType.Type.B : HitType.Type.None;
	}

	public void OnHitC(InputValue value)
	{
		hit = value.isPressed ? HitType.Type.C : HitType.Type.None;
	}
}
