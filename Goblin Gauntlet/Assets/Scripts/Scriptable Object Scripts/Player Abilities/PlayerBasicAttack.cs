using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Basic Attack", menuName = "Scriptable Object/Ability/Player Basic Attack")]
public class PlayerBasicAttack : Ability
{
	private PlayerController playerController;

	public float damage = 1f;
	public float attackRadius = 2.5f;
	public float fieldOfAttack = 45f;

	private List<GameObject> enemiesSeen;
	private GameObject nearestEnemy;

	public bool attacking = false;

	public override void UseAbility(GameObject parent)
	{
		playerController = parent.GetComponent<PlayerController>();

		attacking = true;
		Debug.Log($"{parent.name} basic attack used");

		enemiesSeen = new List<GameObject>();
		nearestEnemy = null;

		Collider[] enemiesInViewRadius = Physics.OverlapSphere(parent.transform.position, attackRadius,
			GameManager.instance.enemyLayer);

		foreach (Collider enemyCollider in enemiesInViewRadius)
		{
			Vector3 directionToEnemy = (enemyCollider.transform.position -
				parent.transform.position).normalized;

			// Check if enemy is within the field of view
			if (Vector3.Angle(parent.transform.forward, directionToEnemy) < fieldOfAttack / 2)
			{
				float distanceToEnemy = Vector3.Distance(parent.transform.position,
					enemyCollider.transform.forward);

				// Check if there are obstructions between the AI and the player
				if (!Physics.Raycast(parent.transform.position, directionToEnemy, distanceToEnemy,
					GameManager.instance.obstructionLayer))
				{
					GameObject enemySeen = enemyCollider.gameObject;
					//Debug.Log($"Enemy detected: {enemySeen.name}");

					if (!enemiesSeen.Contains(enemySeen))
					{
						//Debug.Log($"New enemy detected: {enemySeen.name}");
						enemiesSeen.Add(enemySeen);
					}
				}
			}
		}

		float closestEnemyDistance = Mathf.Infinity;
		foreach (GameObject enemy in enemiesSeen)
		{
			float distanceToEnemy = Vector3.Distance(parent.transform.position, enemy.transform.position);

			// If distanceToEnemy is less than the closestEnemyDistance set closestEnemyDistance
			// to distanceToEnemy
			if (distanceToEnemy < closestEnemyDistance)
			{
				closestEnemyDistance = distanceToEnemy;
				nearestEnemy = enemy;

				//Debug.Log($"nearestEnemy = {nearestEnemy}");
			}
		}

		if (nearestEnemy != null)
		{
			nearestEnemy.GetComponent<IDamageable>().TakeDamage(playerController.damage);
		}
	}

	public override void EndAbility(GameObject parent)
	{
		attacking = false;
		Debug.Log($"{parent.name} basic attack ended");
		nearestEnemy = null;
	}
}
