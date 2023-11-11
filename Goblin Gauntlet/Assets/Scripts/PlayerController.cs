using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
	public PlayableCharacter characterData;

    // Character.cs variables
    private string characterName;
    private float health;
	public float damage;
    private float speed;
    private float rotationSpeed;

    // PlayableCharacter.cs variables
    private CharacterClass characterClass;

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

	private Rigidbody rb;

	public float smoothTime = 0.1f;

	enum AbilityState
	{
		ready,
		active,
		cooldown
	}

    private AbilityState mainAbilityState;
	private AbilityState specialAbilityState;

	// Awake is called before Start
	void Awake()
    {
        // Access character data - Character.cs
        characterName = characterData.characterName;
        health = characterData.health;
		damage = characterData.baseDamage;
        speed = characterData.speed;
        rotationSpeed = characterData.rotationSpeed;

        // Access character data - PlayableCharacter.cs
        characterClass = characterData.characterClass;
        mainAbility = characterData.mainAbility;
        specialAbility = characterData.specialAbility;

		// OTHER VARIABLES //

		// Input actions
		inputActions = new InputActions();

		// Subscribe to Movement action
		inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>().normalized;
		inputActions.Player.Movement.canceled += context => movementInput = Vector2.zero;

		// Subscribe to Look action
		inputActions.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>().normalized;
		inputActions.Player.Look.canceled += context => movementInput = Vector2.zero;

		// Set ability states to ready
		mainAbilityState = AbilityState.ready;
        specialAbilityState = AbilityState.ready;

		rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update()
	{
		// Calculate the movement direction in the camera's perspective
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;			// Removes the y component of the forward vector and normalizes it
		Vector3 movementDirection = movementInput.x * Camera.main.transform.right + movementInput.y * cameraForward;	// giving a forward vector that is always parallel to the ground

		// Move player using velocity
		Vector3 movement = new Vector3(movementDirection.x * speed, rb.velocity.y, movementDirection.z * speed);

		rb.velocity = movement;

		// Handle player rotation
		if (movementInput.sqrMagnitude > 0.01f) // Check if there's input
		{
			// Handle player rotation
			float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
			float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothTime);
			this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
		}

		// Abilities
		CheckAbilityState("MainAbility", mainAbility, ref mainAbilityState, ref mainAbilityCooldownTime, ref mainAbilityActiveTime);
		CheckAbilityState("SpecialAbility", specialAbility, ref specialAbilityState, ref specialAbilityCooldownTime, ref specialAbilityActiveTime);
	}

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

	void Attack()
	{

	}
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
