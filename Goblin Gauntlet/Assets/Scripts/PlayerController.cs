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
        inputActions.Player.Movement.performed += Context => movementInput = Context.ReadValue<Vector2>();

        // Set ability states to ready
        mainAbilityState = AbilityState.ready;
        specialAbilityState = AbilityState.ready;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		// Movement
		

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
				}
				else
				{
					abilityState = AbilityState.ready;
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
