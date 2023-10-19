using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Character", menuName = "Characters/Enemy")]
public class EnemyCharacter : Character
{
	public float damage;
	public List<GameObject> playersSeen;
	public bool attacked;
	public bool attacking;
}
