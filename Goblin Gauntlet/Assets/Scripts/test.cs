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

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Characters/Playable")]
public class PlayableCharacter : Character
{
	// Shared characteristics between playable characters
	public Ability mainAbility;
	public Ability specialAbility;
}

[CreateAssetMenu(fileName = "New Enemy Character", menuName = "Characters/Enemy")]
public class EnemyCharacter : Character
{
	public float damage;
}

[System.Serializable]
public class Ability
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
