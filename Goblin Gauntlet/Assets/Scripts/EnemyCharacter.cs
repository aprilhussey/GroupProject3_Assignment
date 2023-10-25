using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Character", menuName = "Characters/Enemy")]
public class EnemyCharacter : Character
{
	public float dealDamage;
	public float visionDistance;
	public float fieldOfView;
}
