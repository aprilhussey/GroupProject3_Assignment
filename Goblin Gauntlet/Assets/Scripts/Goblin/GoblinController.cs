using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
	public EnemyCharacter characterData;

	private string id;
	private string characterName;
	private int health;
	private float speed;
	private float rotationSpeed;

	private float damage;
	private List<GameObject> playersSeen;
	private GameObject closestPlayer = null;
	private bool attacked;
	private bool attacking;

	public LayerMask playerLayer;
	public LayerMask obstructionLayer;

	// Start is called before the first frame update
	void Start()
	{
		playerLayer = 1 << 3;   // Represents the Player layer in project
		obstructionLayer = 1 << 6;   // Represents the Obstruction layer in project

		// Access character data
		id = characterData.id;
		characterName = characterData.characterName;
		health = characterData.health;
		speed = characterData.speed;
		rotationSpeed = characterData.rotationSpeed;

		damage = characterData.damage;
		playersSeen = characterData.playersSeen;
		closestPlayer = characterData.closestPlayer;
		attacked = characterData.attacked;
		attacking = characterData.attacking;

		characterData.playersSeen.Clear();	// Remove all elements from list
		characterData.playersSeen.TrimExcess();	// Release the memory that was allocated for the removed elements
	}

	public GameObject player;
	private float distanceToClosestPlayer;

	public GameObject artifact; // TESTING
	private float distanceToArtifact;

	private float closerDistance = Mathf.Infinity;	// Start with infinity so the first player distance will always be closer

	void Update()
	{
		distanceToArtifact = CalculateDistanceToArtifact();

		// If playersSeen list is empty
		if (characterData.playersSeen.Count <= 0)
		{
			// Move towards artifact
			Vector3 targetDirection = (artifact.transform.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));
			
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * characterData.rotationSpeed);
			transform.position = Vector3.MoveTowards(transform.position, artifact.transform.position, characterData.speed * Time.deltaTime);
		}

		//If playersSeen list is not empty
		if (characterData.playersSeen.Count > 0)
		{
			characterData.closestPlayer = FindClosestPlayer();
		}
		else
		{
			// Do nothing
		}

		// If closestPlayer has a player
		if (characterData.closestPlayer != null)
		{
			distanceToClosestPlayer = CalculateDistanceToClosestPlayer(characterData.closestPlayer);
			
			if (distanceToClosestPlayer < distanceToArtifact)
			{
				FollowPlayer(characterData.closestPlayer);
			}
		}
	}

	public float CalculateDistanceToArtifact()
	{
		float distanceToArtifact = Vector3.Distance(transform.position, artifact.transform.position);
		Debug.DrawLine(transform.position, artifact.transform.position, Color.blue);
		return distanceToArtifact;
	}

	public GameObject FindClosestPlayer()
	{
		foreach (GameObject player in characterData.playersSeen)
		{
			float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
			Debug.DrawLine(transform.position, player.transform.position, Color.red);

			if (distanceToPlayer < closerDistance)
			{
				closerDistance = distanceToPlayer;
				characterData.closestPlayer = player;
			}
		}
		return characterData.closestPlayer;
	}

	public float CalculateDistanceToClosestPlayer(GameObject closestPlayer)
	{
		float distanceToClosestPlayer = Vector3.Distance(transform.position, characterData.closestPlayer.transform.position);
		Debug.DrawLine(transform.position, characterData.closestPlayer.transform.position, Color.blue);
		return distanceToClosestPlayer;
	}

	public void FollowPlayer(GameObject player)
	{
		Debug.Log("Following player: " + player.name);
		Vector3 targetDirection = (player.gameObject.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * characterData.rotationSpeed);
		transform.position = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, characterData.speed * Time.deltaTime);
	}

	public void StopFollowingPlayer(GameObject player)
	{
		Debug.Log("Player left the follow radius");
		//characterData.playersSeen.Clear();	// Remove all elements from list
		//characterData.playersSeen.TrimExcess();	// Release the memory that was allocated for the removed elements
		characterData.closestPlayer = null;
	}

	public void AttackPlayer(GameObject player)
	{
		Debug.Log("Attacking the player for " + damage + " damage");
	}

	public void StopAttackingPlayer(GameObject player)
	{
		Debug.Log("Player left the attack radius");
	}
}
