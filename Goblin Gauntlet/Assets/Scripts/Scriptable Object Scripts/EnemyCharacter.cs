using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Character", menuName = "Scriptable Object/Character/Enemy")]
public class EnemyCharacter : Character
{
	public float fieldOfView;
	public float visionDistance;
	public float attackDistance;
	//public float attackCooldown;

	public GoblinBasicAttack basicAttack;
}
