using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
	// Shared characteristics between all abilities
	public string abilityName;
	public float activeTime;
	public float cooldownTime;

	public enum AbilityState
	{
		ready,
		active,
		cooldown
	}

	public virtual void UseAbility(GameObject parent) { }

	public virtual void AbilityActive(GameObject parent, AbilityState abilityState) { }

	public virtual void EndAbility(GameObject parent) { }
}
