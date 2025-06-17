using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class InputsManager : MonoBehaviour
{
	[HideInInspector] public static InputsManager Instance { get; private set; }

	[HideInInspector] public Vector2 move;
	[HideInInspector] public HitType.Type hit = HitType.Type.None;
	[HideInInspector] public bool dash;
	[HideInInspector] public bool reloadScene;
	[HideInInspector] public bool interact;
	[HideInInspector] public bool mainMenu;

	[HideInInspector] public bool validate;
	[HideInInspector] public bool cancel;
	[HideInInspector] public float creditScroll;
	[HideInInspector] public bool skip;

	private PlayerInput _playerInput;
	private bool canInput = true;

	public bool CanInput => canInput;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		_playerInput = GetComponent<PlayerInput>();
	}

	public void EnableInput(bool state)
	{
		canInput = state;
		move = Vector2.zero;
	}

	public void SetMap(Map map)
	{
		InputActionMap current = _playerInput.currentActionMap;
		current.Disable();

		switch (map)
		{
			case Map.Gameplay:
				current = _playerInput.actions.FindActionMap("Gameplay");
				break;
			case Map.Menu:
				current = _playerInput.actions.FindActionMap("Menu");
				break;
			default:
				break;
		}

		current.Enable();
	}

	public void OnCreditScroll(InputValue value)
	{
		creditScroll = value.Get<float>();
	}

	public void OnSkip(InputValue value)
	{
		skip = value.isPressed;
	}

	public void OnValidation()
	{
		validate = true;
	}

	public void OnCancel()
	{
		cancel = true;
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

public enum Map
{
	Gameplay,
	Menu,
}