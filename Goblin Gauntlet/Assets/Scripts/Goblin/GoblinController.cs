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
	private List<GameObject> playersSeen;
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

		damage = characterData.damage;
		playersSeen = characterData.playersSeen;
		attacked = characterData.attacked;
		attacking = characterData.attacking;

		characterData.playersSeen.Clear();	// Remove all elements from list
		characterData.playersSeen.TrimExcess();	// Release the memory that was allocated for the removed elements
	}

	public GameObject player;
	public GameObject closestPlayer = null;
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
			Vector3 targetDirection = (artifact.transform.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));
			
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			transform.position = Vector3.MoveTowards(transform.position, artifact.transform.position, speed * Time.deltaTime);
		}

		// If playersSeen list is not empty
		if (characterData.playersSeen.Count > 0)
		{
			closestPlayer = FindClosestPlayer();
		}

		// If closestPlayer has a player
		if (closestPlayer != null)
		{
			distanceToClosestPlayer = CalculateDistanceToClosestPlayer(closestPlayer);
			
			if (distanceToClosestPlayer < distanceToArtifact)
			{
				//FollowPlayer(closestPlayer);
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
				closestPlayer = player;
			}
		}
		return closestPlayer;
	}

	public float CalculateDistanceToClosestPlayer(GameObject closestPlayer)
	{
		float distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.transform.position);
		Debug.DrawLine(transform.position, closestPlayer.transform.position, Color.blue);
		return distanceToClosestPlayer;
	}

	public float speed = 2.0f;
	public float rotationSpeed = 5.0f;

	public void FollowPlayer(GameObject player)
	{
		Debug.Log("Following the player");
		//transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
	}

	public void StopFollowingPlayer(GameObject player)
	{
		Debug.Log("Player left the follow radius");
		characterData.playersSeen.Clear();	// Remove all elements from list
		characterData.playersSeen.TrimExcess();	// Release the memory that was allocated for the removed elements
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
