using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Scriptable Object/Entity/Entity")]
public class Entity : ScriptableObject
{
	// Shared characteristics between all entities
	public string entityName;
	public float health;
}
