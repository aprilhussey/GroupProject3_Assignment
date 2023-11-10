using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Characters/Playable")]
public class PlayableCharacter : Character
{
	// Shared characteristics between playable characters
	public CharacterClass characterClass;
	public Ability mainAbility;
	public Ability specialAbility;
}

// This holds all the possible character class options for a playable character
public enum CharacterClass
{
	Paladin,
	Warlock,
	Cleric,
	Rogue
}
