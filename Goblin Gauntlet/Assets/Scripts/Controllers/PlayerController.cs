using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour, IDamageable
{
	public PlayableCharacter characterData;

	// Entity.cs variables
	private string characterName;
	[HideInInspector]
	public float health;
	[HideInInspector]
	public bool canHeal = true;

	// Character.cs variables
    private float speed;
    private float rotationSpeed;

    // PlayableCharacter.cs variables
    private CharacterClass characterClass;

	[HideInInspector]
	public PlayerBasicAttack basicAttack;
	[HideInInspector]
	public float basicAttackCooldownTime;
	private float basicAttackActiveTime;

	[HideInInspector]
	public Ability mainAbility;
    private float mainAbilityCooldownTime;
	private float mainAbilityActiveTime;

	[HideInInspector]
	public Ability specialAbility;
	private float specialAbilityCooldownTime;
	private float specialAbilityActiveTime;

	// Other variables
	[HideInInspector]
	public float damage;

	private Vector2 movementInput = Vector2.zero;
	private Vector2 lookInput = Vector2.zero;
	// Commented out as this will need to be implemented at some point //
	//private string gamepadControlSchemeName = "Gamepad";
	//private string keyboardMouseControlSchemeName = "KeyboardMouse";
	// Commented out as this will need to be implemented at some point //

	private Rigidbody playerRigidbody;

	public float smoothRotationTime = 0.1f;

	private PlayerInput playerInputComponent;
	// Commented out as this will need to be implemented at some point //
	//private string currentControlScheme;
	// Commented out as this will need to be implemented at some point //

	private Ability.AbilityState basicAttackState;
    private Ability.AbilityState mainAbilityState;
	private Ability.AbilityState specialAbilityState;

	private InputActions inputActions;

	private bool basicAttackInput;
	private bool mainAbilityInput;
	private bool specialAbilityInput;

	//Damage Particle System
	public ParticleSystem playerDamageSpark; 

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

		// Set ability states to ready
		basicAttackState = Ability.AbilityState.ready;
		mainAbilityState = Ability.AbilityState.ready;
        specialAbilityState = Ability.AbilityState.ready;

		basicAttackInput = false;
		mainAbilityInput = false;
		specialAbilityInput = false;

		playerRigidbody = GetComponent<Rigidbody>();

		playerInputComponent = GetComponent<PlayerInput>();

		// Input actions
		inputActions = new InputActions();
		inputActions.Enable();
	}

	// Update is called once per frame
	void Update()
	{
		if (!GameManager.isGamePaused)
		{
			// Commented out as this will need to be implemented at some point //
			//currentControlScheme = playerInputComponent.currentControlScheme;
			//Debug.Log($"currentControlScheme = {currentControlScheme}");
			// Commented out as this will need to be implemented at some point //

			Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;          // Removes the y component of the forward vector and normalizes it
																															// giving a forward vector that is always parallel to the ground
			// Calculate the movement direction in the camera's perspective
			Vector3 movementDirection = movementInput.x * Camera.main.transform.right + movementInput.y * cameraForward;

			// Move player using velocity
			Vector3 movement = new Vector3(movementDirection.x * speed, playerRigidbody.velocity.y, movementDirection.z * speed);

			playerRigidbody.velocity = movement;

			// Handle player rotation
			if (movementInput.sqrMagnitude > 0.01f) // Check if there's input
			{
				playerRigidbody.velocity = movement;

				// Use gamepad controls
				float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
				float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothRotationTime);
				this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

				// Commented out as this will need to be implemented at some point //
				/*if (currentControlScheme == "Gamepad")
				{
					// Use gamepad controls
					float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
					float smoothedAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref rotationSpeed, smoothRotationTime);
					this.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

				}
				if (currentControlScheme == "KeyboardMouse")
				{
					// Use keyboard and mouse controls
				}*/
				// Commented out as this will need to be implemented at some point //
			}

			if (movementInput.sqrMagnitude < 0.01f)	// If no movementInput is detected...
			{
				// ... set movementInput and playerRigidbody.angularVelocity to zero
				movementInput = Vector2.zero;
				playerRigidbody.angularVelocity = Vector3.zero;
			}

			// If player health is less than or equal to 0
			if (health <= 0)
			{
				//Debug.Log($"{gameObject.name} destroyed");
				Destroy(gameObject);
			}
			Debug.Log($"{gameObject.name} health = {health}");
		}

		CheckAbilityState(ref basicAttackInput, basicAttack, ref basicAttackState, ref basicAttackCooldownTime, ref basicAttackActiveTime);
		//CheckAbilityState(ref mainAbilityInput, mainAbility, ref mainAbilityState, ref mainAbilityCooldownTime, ref mainAbilityActiveTime);
		//CheckAbilityState(ref specialAbilityInput, specialAbility, ref specialAbilityState, ref specialAbilityCooldownTime, ref specialAbilityActiveTime);
	}

	// Class needs to derive from 'IDamageable' for this function to work
	public void TakeDamage(float amount)
	{
		if (health > 0)
		{
			playerDamageSpark.Play();
			health -= amount;
		}
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		movementInput = context.ReadValue<Vector2>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		lookInput = context.ReadValue<Vector2>();
	}

	public void OnBasicAttack(InputAction.CallbackContext context)
	{
		basicAttackInput = context.action.triggered;
		CheckAbilityState(ref basicAttackInput, basicAttack, ref basicAttackState, ref basicAttackCooldownTime, ref basicAttackActiveTime);
	}

	public void OnMainAbility(InputAction.CallbackContext context)
	{
		//mainAbilityInput = context.action.triggered;
		//CheckAbilityState(ref mainAbilityInput, mainAbility, ref mainAbilityState, ref mainAbilityCooldownTime, ref mainAbilityActiveTime);
	}

	public void OnSpecialAbility(InputAction.CallbackContext context)
	{
		//specialAbilityInput = context.action.triggered;
		//CheckAbilityState(ref specialAbilityInput, specialAbility, ref specialAbilityState, ref specialAbilityCooldownTime, ref specialAbilityActiveTime);
	}

	void CheckAbilityState(ref bool abilityInput, Ability ability, ref Ability.AbilityState abilityState, ref float abilityCooldownTime, ref float abilityActiveTime)
    {
		switch (abilityState)
		{
			case Ability.AbilityState.ready:
				if (abilityInput)
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
