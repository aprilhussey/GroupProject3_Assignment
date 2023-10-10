using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Characters/Playable")]
public class PlayableCharacter : Character
{
	// Shared characteristics between playable characters
	public Ability mainAbility;
	public Ability specialAbility;
}