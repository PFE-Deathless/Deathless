using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
	[HideInInspector] public static InputsManager Instance { get; private set; }


	[HideInInspector] public Vector2 move;
	[HideInInspector] public HitType.Type hit = HitType.Type.None;
	[HideInInspector] public bool dash;
	[HideInInspector] public bool reloadScene;
	[HideInInspector] public bool interact;
	[HideInInspector] public bool mainMenu;

	private bool canInput = true;

	public bool CanInput => canInput;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void EnableInput(bool state)
	{
		canInput = state;
		move = Vector2.zero;
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
		canInput = true;
		reloadScene = true;
	}

	public void OnInteract()
	{
		if (canInput)
			interact = true;
		else
			interact = false;
	}

	public void OnMainMenu()
	{
		canInput = true;
		mainMenu = true;
	}
}
