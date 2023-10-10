using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
	public EnemyCharacter characterData;

	private string id;
	private string characterName;
	private int health;
	private float damage;

	// Start is called before the first frame update
	void Start()
	{
		// Access character data
		id = characterData.id;
		characterName = characterData.characterName;
		health = characterData.health;
		damage = characterData.damage;
	}

	public GameObject player;   // TESTING

	public void FollowPlayer()
	{
		Debug.Log("Following the player");
	}

	public void StopFollowingPlayer()
	{
		Debug.Log("Player left the follow radius");
	}

	public void AttackPlayer()
	{
		Debug.Log("Attacking the player for " + damage + " damage");
	}

	public void StopAttackingPlayer()
	{
		Debug.Log("Player left the attack radius");
	}
}
