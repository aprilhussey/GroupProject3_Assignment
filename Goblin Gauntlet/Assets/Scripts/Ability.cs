using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
	public string id;
	public string name;
	public int damage;
	public float cooldown;

	public void UseAbility(Character target)
	{
		target.TakeDamage(damage);
		// Add code to start cooldown
	}
}
