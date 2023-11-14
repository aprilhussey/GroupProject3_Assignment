using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, IDamageable
{
	public PlayableCharacter characterData;

	// Entity.cs variables
	private string characterName;
	[HideInInspector] public float health;

	// Character.cs variables
    private float speed;
    private float rotationSpeed;

    // PlayableCharacter.cs variables
    private CharacterClass characterClass;

	private PlayerBasicAttack basicAttack;
	[HideInInspector] public float basicAttackCooldownTime;
	private float basicAttackActiveTime;

	private Ability mainAbility;
    private float mainAbilityCooldownTime;
	private float mainAbilityActiveTime;

	private Ability specialAbility;
	private float specialAbilityCooldownTime;
	private float specialAbilityActiveTime;

	// Other variables
	[HideInInspector] public float damage;

	private InputActions inputActions;
	private Vector2 movementInput = new Vector2();
	private Vector2 lookInput = new Vector2();

	//private string gamepadControlSchemeName = "Gamepad";
	//private string keyboardMouseControlSchemeName = "KeyboardMouse";

	private Rigidbody rb;

	public float smoothRotationTime = 0.1f;

	private PlayerInput playerInputComponent;
	private string currentControlScheme;

	private Ability.AbilityState basicAttackState;
    private Ability.AbilityState mainAbilityState;
	private Ability.AbilityState specialAbilityState;

	// Awake is called before Start
	void Awake()
    {
        // Access character data - Entity.cs
        characterName = characterData.entityName;
        health = characterData.health;

		// Access character data - Character.cs
		damage = characterData.basicAttack.damage;
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
		basicAttackState = Ability.AbilityState.ready;
		mainAbilityState = Ability.AbilityState.ready;
        specialAbilityState = Ability.AbilityState.ready;

		rb = GetComponent<Rigidbody>();

		playerInputComponent = GetComponent<PlayerInput>();
	}

	void OnEnable()
	{
		inputActions.Enable();
	}

	void OnDisable()
	{
		inputActions.Disable();
	}

	// Update is called once per frame
	void Update()
	{
		if (!GameManager.isGamePaused)
		{
			currentControlScheme = playerInputComponent.currentControlScheme;
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
				rb.velocity = movement;

				if (currentControlScheme == "Gamepad")
				{
					// Use gamepad controls
					float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
					float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothRotationTime);
					this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

				}
				if (currentControlScheme == "KeyboardMouse")
				{
					// Use keyboard and mouse controls
				}
			}
			else
			{
				//rotationSpeed = 0f;	// Reset rotation when there's no input
			}

			// Abilities
			CheckAbilityState("BasicAttack", basicAttack, ref basicAttackState, ref basicAttackCooldownTime, ref basicAttackActiveTime);
			CheckAbilityState("MainAbility", mainAbility, ref mainAbilityState, ref mainAbilityCooldownTime, ref mainAbilityActiveTime);
			CheckAbilityState("SpecialAbility", specialAbility, ref specialAbilityState, ref specialAbilityCooldownTime, ref specialAbilityActiveTime);

			// If player health is less than or equal to 0
			if (health <= 0)
			{
				Debug.Log("Artifact destroyed");
				Destroy(gameObject);
			}

			Debug.Log($"{gameObject.name} health = {health}");
		}
	}


	// Class needs to derive from 'IDamageable' for this function to work
	public void TakeDamage(float amount)
	{
		if (health > 0)
		{
			health -= amount;
		}
	}

	void CheckAbilityState(string inputActionName, Ability ability, ref Ability.AbilityState abilityState, ref float abilityCooldownTime, ref float abilityActiveTime)
    {
		InputAction inputAction = GetInputAction(inputActionName);

		switch (abilityState)
		{
			case Ability.AbilityState.ready:
				if (inputAction.triggered)
				{
					ability.UseAbility(this.gameObject);
					abilityState = Ability.AbilityState.active;
					abilityActiveTime = ability.activeTime;
				}
				break;

			case Ability.AbilityState.active:
				if (abilityActiveTime > 0)
				{
					abilityActiveTime -= Time.deltaTime;
					//Debug.Log($"{ability.abilityName} active time = {abilityActiveTime}");
				}
				else
				{
					abilityState = Ability.AbilityState.cooldown;
					abilityCooldownTime = ability.cooldownTime;
				}
				break;

			case Ability.AbilityState.cooldown:
				if (abilityCooldownTime > 0)
				{
					abilityCooldownTime -= Time.deltaTime;
					//Debug.Log($"{ability.abilityName} cooldown time = {abilityCooldownTime}");
				}
				else
				{
					abilityState = Ability.AbilityState.ready;
					ability.EndAbility(this.gameObject);
				}
				break;
		}
	}

	public InputAction GetInputAction(string inputActionName)
	{
		switch (inputActionName)
		{
			case "BasicAttack":
				return inputActions.Player.BasicAttack;

			case "MainAbility":
				return inputActions.Player.MainAbility;

			case "SpecialAbility":
				return inputActions.Player.SpecialAbility;

			default:
				throw new ArgumentException($"Non-existent input action: {inputActionName}");
		}
	}

	/*void OnDrawGizmos()
	{
		// Draw field of attack
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -basicAttack.fieldOfAttack / 2, 0) * transform.forward
			* basicAttack.attackRadius);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, basicAttack.fieldOfAttack / 2, 0) * transform.forward
			* basicAttack.attackRadius);

		// Draw attack radius
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, basicAttack.attackRadius);
	}*/
}
