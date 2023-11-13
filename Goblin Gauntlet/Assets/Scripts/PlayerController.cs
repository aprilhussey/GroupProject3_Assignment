using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
	public PlayableCharacter characterData;

    // Character.cs variables
    private string characterName;
    private float health;
	[HideInInspector] public float damage;
    private float speed;
    private float rotationSpeed;

    // PlayableCharacter.cs variables
    private CharacterClass characterClass;

	private Ability basicAttack;
	[HideInInspector] public float basicAttackCooldownTime;
	private float basicAttackActiveTime;

	private Ability mainAbility;
    private float mainAbilityCooldownTime;
	private float mainAbilityActiveTime;

	private Ability specialAbility;
	private float specialAbilityCooldownTime;
	private float specialAbilityActiveTime;

	// Other variables
	private InputActions inputActions;
	private Vector2 movementInput = new Vector2();
	private Vector2 lookInput = new Vector2();

	private string gamepadControlSchemeName = "Gamepad";
	private string keyboardMouseControlSchemeName = "KeyboardMouse";

	private Rigidbody rb;

	public float smoothTime = 0.1f;

	public PlayerInput playerInputComponent;
	private string currentControlScheme;

	enum AbilityState
	{
		ready,
		active,
		cooldown
	}

	private AbilityState basicAttackState;
    private AbilityState mainAbilityState;
	private AbilityState specialAbilityState;

	// Awake is called before Start
	void Awake()
    {
        // Access character data - Character.cs
        characterName = characterData.characterName;
        health = characterData.health;
		damage = characterData.damage;
        speed = characterData.speed;
        rotationSpeed = characterData.rotationSpeed;

        // Access character data - PlayableCharacter.cs
        characterClass = characterData.characterClass;
		basicAttack = characterData.basicAttack;
		mainAbility = characterData.mainAbility;
        specialAbility = characterData.specialAbility;

		// OTHER VARIABLES //

		// Input actions
		inputActions = new InputActions();

		// Subscribe to Movement action
		inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
		inputActions.Player.Movement.canceled += context => movementInput = Vector2.zero;

		// Subscribe to Look action
		inputActions.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>();
		inputActions.Player.Look.canceled += context => movementInput = Vector2.zero;

		// Set ability states to ready
		basicAttackState = AbilityState.ready;
		mainAbilityState = AbilityState.ready;
        specialAbilityState = AbilityState.ready;

		rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame
	void Start()
	{
		//playerInputComponent = GetComponent<PlayerInput>();
	}

	// Update is called once per frame
	void Update()
	{
		//currentControlScheme = playerInputComponent.currentControlScheme;
		//Debug.Log($"currentControlScheme = {currentControlScheme}");

		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;          // Removes the y component of the forward vector and normalizes it
																														// giving a forward vector that is always parallel to the ground
		// Calculate the movement direction in the camera's perspective
		Vector3 movementDirection = movementInput.x * Camera.main.transform.right + movementInput.y * cameraForward;

		// Move player using velocity
		Vector3 movement = new Vector3(movementDirection.x * speed, rb.velocity.y, movementDirection.z * speed);

		rb.velocity = movement;

		// Handle player rotation
		if (movementInput.sqrMagnitude > 0.01f) // Check if there's input
		{
			// Handle player rotation
			//float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
			//float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothTime);
			//this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

			if (currentControlScheme == "Gamepad")
			{
				// Use gamepad controls
				float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
				float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothTime);
				this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
			}
			if (currentControlScheme == "KeyboardMouse")
			{
				// Use keyboard and mouse controls
			}
		}

		// Abilities
		CheckAbilityState("BasicAttack", basicAttack, ref basicAttackState, ref basicAttackCooldownTime, ref basicAttackActiveTime);
		CheckAbilityState("MainAbility", mainAbility, ref mainAbilityState, ref mainAbilityCooldownTime, ref mainAbilityActiveTime);
		CheckAbilityState("SpecialAbility", specialAbility, ref specialAbilityState, ref specialAbilityCooldownTime, ref specialAbilityActiveTime);
	}

	void LateUpdate()
	{
		//currentControlScheme = playerInputComponent.currentControlScheme;
		//Debug.Log($"currentControlScheme: {currentControlScheme}");
	}

	void OnEnable()
    {
        inputActions.Enable();
		//playerInputComponent.onActionTriggered += OnActionTriggered;
	}

    void OnDisable()
    {
        inputActions.Disable();
		//playerInputComponent.onActionTriggered -= OnActionTriggered;
	}

	/*void OnActionTriggered(InputAction.CallbackContext context)
	{
		var device = context.control.device;
		Debug.Log($"device = {device}");

		if (device != null)
		{
			if (device is Gamepad)
			{
				playerInputComponent.SwitchCurrentControlScheme(gamepadControlSchemeName, device);
			}

			if (device is Keyboard || device is Mouse)
			{
				playerInputComponent.SwitchCurrentControlScheme(keyboardMouseControlSchemeName, device);
			}
		}
	}*/

    void CheckAbilityState(string inputActionName, Ability ability, ref AbilityState abilityState, ref float abilityCooldownTime, ref float abilityActiveTime)
    {
		InputAction inputAction = GetInputAction(inputActionName);

		switch (abilityState)
		{
			case AbilityState.ready:
				if (inputAction.triggered)
				{
					ability.UseAbility(this.gameObject);
					abilityState = AbilityState.active;
					abilityActiveTime = ability.activeTime;
				}
				break;

			case AbilityState.active:
				if (abilityActiveTime > 0)
				{
					ability.AbilityActive(this.gameObject, abilityState);
					abilityActiveTime -= Time.deltaTime;
					Debug.Log($"{ability.abilityName} active time = {abilityActiveTime}");
				}
				else
				{
					abilityState = AbilityState.cooldown;
					abilityCooldownTime = ability.cooldownTime;
				}
				break;

			case AbilityState.cooldown:
				if (abilityCooldownTime > 0)
				{
					abilityCooldownTime -= Time.deltaTime;
					Debug.Log($"{ability.abilityName} cooldown time = {abilityCooldownTime}");
				}
				else
				{
					abilityState = AbilityState.ready;
					ability.EndAbility(this.gameObject);
				}
				break;
		}
	}

	public InputAction GetInputAction(string inputActionName)
	{
		switch (inputActionName)
		{
			case "SpecialAbility":
				return inputActions.Player.SpecialAbility;

			case "MainAbility":
				return inputActions.Player.MainAbility;

			default:
				throw new ArgumentException($"Non-existent input action: {inputActionName}");
		}
	}
}
