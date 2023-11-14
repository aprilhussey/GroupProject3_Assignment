using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoblinController : MonoBehaviour, IDamageable
{
    public EnemyCharacter characterData;

    // Entity.cs varaibles
    private string characterName;
    [HideInInspector] public float health;

	// Character.cs varaibles
	private float speed;
	private float rotationSpeed;

	// EnemyCharacter.cs variables
	private float fieldOfView;
	private float visionDistance;
	private float attackDistance;
	private float attackCooldown;

	private GoblinBasicAttack basicAttack;
	[HideInInspector] public float basicAttackCooldownTime;
	private float basicAttackActiveTime;

	// Sphere collider variables
	private SphereCollider[] sphereColliders;
    private SphereCollider followRadiusCollider;
    private SphereCollider attackRadiusCollider;

	// Other variables
	private List<GameObject> playersSeen;
	[HideInInspector] public GameObject target = null;
	private bool targetInAttackRadius;
	//private bool attacked;
	//private bool attacking;

	private GameObject nearestPlayer;   // Player compared to it will always be closer
	private GameObject artifact;

	private float targetHealth;

	private Ability.AbilityState basicAttackState;

	[SerializeField] FloatingHealthBar healthBar;

	// Awake is called before Start
	void Awake()
    {
		// Access character data - Entity.cs
		characterName = characterData.entityName;
		health = characterData.health;

		// Access character data - Character.cs
		speed = characterData.speed;
		rotationSpeed = characterData.rotationSpeed;

		// Access character data - EnemyCharacter.cs
		fieldOfView = characterData.fieldOfView;
		visionDistance = characterData.visionDistance;
		attackDistance = characterData.attackDistance;
		basicAttack = characterData.basicAttack;
		//attackCooldown = characterData.attackCooldown;

		// Get all sphere collider components in children
		sphereColliders = GetComponentsInChildren<SphereCollider>();
		followRadiusCollider = null;
		attackRadiusCollider = null;

		playersSeen = new List<GameObject>();

		// Get artifact game object
		artifact = GameObject.FindGameObjectWithTag("Artifact");

		basicAttackState = Ability.AbilityState.ready;

		healthBar = GetComponentInChildren<FloatingHealthBar>();
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

			if (target == nearestPlayer)
			{
				targetHealth = target.GetComponent<PlayerController>().health;
			}

			if (target == artifact)
			{
				targetHealth = target.GetComponent<ArtifactController>().health;
			}

			if (targetHealth <= 0)
			{ 
				target = null;
			}
		}

		// If goblin health is less than or equal to 0
		if (health <= 0)
		{
			Debug.Log("Goblin dead");
			Destroy(gameObject);
		}

		Debug.Log($"{gameObject.name} health = {health}");

		CheckAbilityState(basicAttack, ref basicAttackState, ref basicAttackCooldownTime, ref basicAttackActiveTime);
	}

	void CheckAbilityState(Ability ability, ref Ability.AbilityState abilityState, ref float abilityCooldownTime, ref float abilityActiveTime)
	{
		switch (abilityState)
		{
			case Ability.AbilityState.ready:
				if (targetInAttackRadius)
				{
					ability.UseAbility(this.gameObject);
					abilityState = Ability.AbilityState.active;
					abilityActiveTime = ability.activeTime;
				}
				break;

			case Ability.AbilityState.active:
				if (abilityActiveTime > 0)
				{
					abilityActiveTime -= Time.deltaTime;
					//Debug.Log($"{ability.abilityName} active time = {abilityActiveTime}");
				}
				else
				{
					abilityState = Ability.AbilityState.cooldown;
					abilityCooldownTime = ability.cooldownTime;
				}
				break;

			case Ability.AbilityState.cooldown:
				if (abilityCooldownTime > 0)
				{
					abilityCooldownTime -= Time.deltaTime;
					//Debug.Log($"{ability.abilityName} cooldown time = {abilityCooldownTime}");
				}
				else
				{
					abilityState = Ability.AbilityState.ready;
					ability.EndAbility(this.gameObject);
				}
				break;
		}
	}

	// AIVision handles the vision of the goblins. It determines how and if a player can be seen by them
	void AIVision()
    {
		Collider[] playersInViewRadius = Physics.OverlapSphere(transform.position, 
			visionDistance, GameManager.instance.playerLayer);

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
					GameManager.instance.obstructionLayer))
				{
					GameObject playerSeen = playerCollider.gameObject;
					//Debug.Log($"Player detected: {playerSeen.name}");

					if (playerSeen.GetComponent<PlayerController>().health > 0) // Check that the player still has health
					{
						if (!playersSeen.Contains(playerSeen))
						{
							//Debug.Log($"New player detected: {playerSeen.name}");
							playersSeen.Add(playerSeen);
						}
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
			//Debug.Log($"playersSeen: {player.name}");

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
		// distanceToArtifact
		float distanceToArtifact = Vector3.Distance(transform.position, artifact.transform.position);
		Debug.DrawLine(transform.position, artifact.transform.position, Color.cyan);

		if (closestPlayerDistance < distanceToArtifact)
		{
			target = nearestPlayer;
		}
		else
		{
			if (artifact.GetComponent<ArtifactController>().health > 0) // Check that the artifact still has health
			{
				target = artifact;
			}
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
			//Debug.Log($"Game object entered follow radius: {other.name}");
		}
	}

	// Handles the OnTriggerExit functionality of the followRadiusCollider
	public void FollowRadiusExited(GameObject other)
	{
		if (playersSeen.Contains(other))
		{
			//Debug.Log($"Game object exited follow radius: {other.name}");
			target = null;
			playersSeen.Remove(other);
		}
	}

	// Handles the OnTriggerEnter functionality of the attackRadiusCollider
	public void AttackRadiusEntered(GameObject other)
	{
		if (other == target)
		{
			//Debug.Log($"Game object entered attack radius: {other.name}");
			targetInAttackRadius = true;
		}
	}

	// Handles the OnTriggerExit functionality of the attackRadiusCollider
	public void AttackRadiusExited(GameObject other)
	{
		if (other == target)
		{
			//Debug.Log($"Game object exited attack radius: {other.name}");
			targetInAttackRadius = false;
		}
	}

	// Class needs to derive from 'IDamageable' for this function to work
	public void TakeDamage(float amount)
	{
		if (health > 0)
		{
			health -= amount;
			healthBar.UpdateHealthBar();
		}
	}

	// CheckIfNull handles checking if any varaibles that shouldn't be null are null and logs a
	// warning message
	void CheckIfNull()
    {
		// Check if Character.cs variables are null
		if (characterName == null)
		{
			Debug.LogWarning($"WARNING: {gameObject.name}.characterName is null");
		}
		if (health == 0)
		{
			Debug.LogWarning($"WARNING:  {gameObject.name}..health is null");
		}
		if (speed == 0)
		{
			Debug.LogWarning($"WARNING:  {gameObject.name}..speed is null");
		}
		if (rotationSpeed == 0)
		{
			Debug.LogWarning($"WARNING:  {gameObject.name}..rotationSpeed is null");
		}

		// Check if EnemyCharacter.cs variables are null
		if (visionDistance == 0)
		{
			Debug.LogWarning($"WARNING:  {gameObject.name}..visionDistance is null");
		}
		if (fieldOfView == 0)
		{
			Debug.LogWarning($"WARNING:  {gameObject.name}..fieldOfView is null");
		}

		// Check if sphere collider varaibles are null
		if (sphereColliders == null)
		{
			Debug.LogWarning($"WARNING: sphereColliders does not contain any sphere colliders");
		}
		if (followRadiusCollider == null)
		{
			Debug.LogError($"ERROR: A game object with a sphere collider not tagged as " +
					"'FollowRadius' was found. Is the tag on the child game " +
					"object set correctly?");
		}
		if (attackRadiusCollider == null)
		{
			Debug.LogError($"ERROR: A game object with a sphere collider not tagged as " +
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
