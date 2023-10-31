using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public EnemyCharacter characterData;

    // Character.cs varaibles
    private string id;
    private string characterName;
    private float health;
    private float speed;
    private float rotationSpeed;

	// EnemyCharacter.cs variables
	private float fieldOfView;
	private float visionDistance;
	private float attackDistance;
	private float attackCooldown;
	private float dealDamage;

	// Layer mask variables
	private LayerMask playerLayer;
    private LayerMask obstructionLayer;

    // Sphere collider variables
    private SphereCollider[] sphereColliders;
    private SphereCollider followRadiusCollider;
    private SphereCollider attackRadiusCollider;

	// Other variables
	private List<GameObject> playersSeen;
	private GameObject target = null;
	private bool targetInAttackRadius;
	//private bool attacked;
	//private bool attacking;

	private GameObject nearestPlayer;   // Player compared to it will always be closer
	private GameObject artifact;

	// FOR TESTING ONLY
	// FOR TESTING ONLY
	// FOR TESTING ONLY
	// FOR TESTING ONLY

	// Awake is called before Start
	void Awake()
    {
		// Access character data - Character.cs
		id = characterData.id;
		characterName = characterData.characterName;
		health = characterData.health;
		speed = characterData.speed;
		rotationSpeed = characterData.rotationSpeed;

		// Access character data - EnemyCharacter.cs
		fieldOfView = characterData.fieldOfView;
		visionDistance = characterData.visionDistance;
		attackDistance = characterData.attackDistance;
		attackCooldown = characterData.attackCooldown;
		dealDamage = characterData.dealDamage;

		// Set layer mask variables to their respective layers
		playerLayer = LayerMask.GetMask("Player");
		obstructionLayer = LayerMask.GetMask("Obstruction");

		// Get all sphere collider components in children
		sphereColliders = GetComponentsInChildren<SphereCollider>();
		followRadiusCollider = null;
		attackRadiusCollider = null;

		playersSeen = new List<GameObject>();

		// Get artifact game object
		artifact = GameObject.FindGameObjectWithTag("Artifact");
	}
    
    // Start is called before the first frame update
	void Start()
    {
		sphereColliders = GetComponentsInChildren<SphereCollider>();

		// Loop through colliders
		foreach (SphereCollider sphereCollider in sphereColliders)
        {
            // Check the tag of the game object the collider is attached to
            if (sphereCollider.gameObject.tag == "FollowRadius")
            {
                followRadiusCollider = sphereCollider;
				followRadiusCollider.radius = visionDistance;
            }

			if (sphereCollider.gameObject.tag == "AttackRadius")
			{
				attackRadiusCollider = sphereCollider;
				attackRadiusCollider.radius = attackDistance;
			}
        }
		CheckIfNull();
    }

    // Update is called once per frame
    void Update()
    {
		AIVision();

		FindTarget();

		if (target != null)
		{
			MoveTowardsTarget(target);
			AttackTarget(target);
		}
	}

	// AIVision handles the vision of the goblins. It determines how and if a player can be seen by them
    void AIVision()
    {
		Collider[] playersInViewRadius = Physics.OverlapSphere(transform.position, 
			visionDistance, playerLayer);

        foreach (Collider playerCollider in playersInViewRadius)
        {
			Vector3 directionToPlayer = (playerCollider.transform.position -
				transform.position).normalized;

			// Check if player is within field of view
			if (Vector3.Angle(transform.forward, directionToPlayer) < fieldOfView / 2)
			{
				float distanceToPlayer = Vector3.Distance(transform.position, 
					playerCollider.transform.position);

				// Check if there are obstructions between the AI and the player
				if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, 
					obstructionLayer))
				{
					GameObject playerSeen = playerCollider.gameObject;
					Debug.Log("Player detected: " + playerSeen.name);

					if (!playersSeen.Contains(playerSeen))
					{
						Debug.Log("New player detected: " + playerSeen.name);
						playersSeen.Add(playerSeen);
					}
				}
			}
        }
    }

	void FindTarget()
	{
		float closestPlayerDistance = Mathf.Infinity;
		foreach (GameObject player in playersSeen)
		{
			Debug.Log("playersSeen: " + player.name);

			float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
			Debug.DrawLine(transform.position, player.transform.position, Color.red);

			// If distanceToPlayer is less than the closestPlayerDistance set closestPlayerDistance
			// to distanceToPlayer
			if (distanceToPlayer < closestPlayerDistance)
			{
				closestPlayerDistance = distanceToPlayer;
				nearestPlayer = player;
			}
		}

		// When closest player to goblin is found check if closestPlayerDistance is less than
		// distanceToArtefact
		float distanceToArtifact = Vector3.Distance(transform.position, artifact.transform.position);
		Debug.DrawLine(transform.position, artifact.transform.position, Color.cyan);

		if (closestPlayerDistance < distanceToArtifact)
		{
			target = nearestPlayer;
		}
		else
		{
			target = artifact;
		}
	}

	void MoveTowardsTarget(GameObject target)
	{
		Vector3 targetDirection = (target.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, 
			targetDirection.z));

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 
			rotationSpeed);
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
			speed * Time.deltaTime);
	}

	// Handles the OnTriggerEnter functionality of the followRadiusCollider
	public void FollowRadiusEntered(GameObject other)
	{
		if (playersSeen.Contains(other))
		{
			Debug.Log("Game object entered follow radius: " + other.name);
		}
	}

	// Handles the OnTriggerExit functionality of the followRadiusCollider
	public void FollowRadiusExited(GameObject other)
	{
		if (playersSeen.Contains(other))
		{
			Debug.Log("Game object exited follow radius: " + other.name);
			target = null;
			playersSeen.Remove(other);
		}
	}

	void AttackTarget(GameObject target)
	{
		if (targetInAttackRadius)
		{
			Debug.Log(gameObject.name + " attacked " + target.name + " for " + dealDamage + " damage");
		}
	}

	// Handles the OnTriggerEnter functionality of the attackRadiusCollider
	public void AttackRadiusEntered(GameObject other)
	{
		if (other == target)
		{
			Debug.Log("Game object entered attack radius: " + other.name);
			targetInAttackRadius = true;
		}
	}

	// Handles the OnTriggerExit functionality of the attackRadiusCollider
	public void AttackRadiusExited(GameObject other)
	{
		if (other == target)
		{
			Debug.Log("Game object exited attack radius: " + other.name);
			targetInAttackRadius = false;
		}
	}

	// CheckIfNull handles checking if any varaibles that shouldn't be null are null and logs a
	// warning message
	void CheckIfNull()
    {
        // Check if Character.cs variables are null
        if (id == null)
        {
            Debug.LogWarning("WARNING: " + gameObject.name + ".id is null");
        }
		if (characterName == null)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".characterName is null");
		}
		if (health == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".health is null");
		}
		if (speed == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".speed is null");
		}
		if (rotationSpeed == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".rotationSpeed is null");
		}

		// Check if EnemyCharacter.cs variables are null
		if (dealDamage == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".damage is null");
		}
		if (visionDistance == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".visionDistance is null");
		}
		if (fieldOfView == 0)
		{
			Debug.LogWarning("WARNING: " + gameObject.name + ".fieldOfView is null");
		}

		// Check if layer mask variables are empty
		if (playerLayer.value == 0)
		{
			Debug.LogWarning("WARNING: playerLayer does not have a layer selected");
		}
		if (obstructionLayer.value == 0)
		{
			Debug.LogWarning("WARNING: obstructionLayer does not have a layer selected");
		}

		// Check if sphere collider varaibles are null
		if (sphereColliders == null)
		{
			Debug.LogWarning("WARNING: sphereColliders does not contain any sphere colliders");
		}
		if (followRadiusCollider == null)
		{
			Debug.LogError("ERROR: A game object with a sphere collider not tagged as " +
					"'FollowRadius' was found. Is the tag on the child game " +
					"object set correctly?");
		}
		if (attackRadiusCollider == null)
		{
			Debug.LogError("ERROR: A game object with a sphere collider not tagged as " +
					"'AttackRadius' was found. Is the tag on the child game " +
					"object set correctly?");
		}
	}

	// Draw gizmo in editor
	void OnDrawGizmos()
	{
		// Draw field of view
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -characterData.fieldOfView / 2, 0) * transform.forward
			* characterData.visionDistance);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, characterData.fieldOfView / 2, 0) * transform.forward
			* characterData.visionDistance);

		// Draw vision distance
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, characterData.visionDistance);

		// Draw attack distance
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, characterData.attackDistance);
	}
}
