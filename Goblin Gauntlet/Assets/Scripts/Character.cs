using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : ScriptableObject
{
	// Shared characteristics between all characters
	public string id;
	public string characterName;
	public int health;

	public virtual void TakeDamage(int amount)
	{
		health -= amount;
	}
}
