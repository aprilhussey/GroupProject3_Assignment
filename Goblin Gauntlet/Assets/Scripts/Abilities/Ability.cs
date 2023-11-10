using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
	// Shared characteristics between all abilities
	public string abilityName;
	public float cooldownTime;
	public float activeTime;

	public virtual void UseAbility(GameObject parent) {}
}
