using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
	[HideInInspector] public static InputsManager Instance { get; private set; }

	[HideInInspector] public bool canInput = true;

	[HideInInspector] public Vector2 move;
	[HideInInspector] public HitType.Type hit;
	[HideInInspector] public bool dash;
	[HideInInspector] public bool reloadScene;
	[HideInInspector] public bool interact;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void OnMove(InputValue value)
	{
		if (canInput)
			move = value.Get<Vector2>().normalized;
		else
			move = Vector2.zero;
	}

	public void OnHitA()
	{
		if (canInput)
			hit = HitType.Type.A;
		else
			hit = HitType.Type.None;
	}

	public void OnHitB()
	{
		if (canInput)
			hit = HitType.Type.B;
		else
			hit = HitType.Type.None;
	}

	public void OnHitC()
	{
		if (canInput)
			hit = HitType.Type.C;
		else
			hit = HitType.Type.None;
	}

	public void OnDash()
	{
		if (canInput)
			dash = true;
		else
			dash = false;
	}

	public void OnReloadScene()
	{
		if (canInput)
			reloadScene = true;
		else
			reloadScene = false;
	}

	public void OnInteract()
	{
		if (canInput)
			interact = true;
		else
			interact = false;
	}
}
