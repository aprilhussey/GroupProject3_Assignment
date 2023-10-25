using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Character", menuName = "Characters/EnemyOld")]
public class EnemyCharacterOld : Character
{
	public float damage;
	public List<GameObject> playersSeen;
	public GameObject closestPlayer;
	public bool attacked;
	public bool attacking;
}
