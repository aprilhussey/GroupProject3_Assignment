using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Poisonous Talons", menuName = "Scriptable Object/Ability/Poisonious Talons")]
public class PoisonousTalons : Ability
{
    private PlayerController playerController;

	public float poisonDamage = 1f;
	public float poisonDuration = 5f;

	public float attackRadius = 2.5f;
	public float fieldOfAttack = 45f;

	private List<GameObject> enemiesSeen;
	private GameObject nearestEnemy;

	public override void UseAbility(GameObject parent)
    {
		playerController = parent.GetComponent<PlayerController>();

		//Debug.Log($"{parent.name} special attack used");

		enemiesSeen = new List<GameObject>();
		nearestEnemy = null;

		Collider[] enemiesInViewRadius = Physics.OverlapSphere(parent.transform.position, attackRadius,
			GameManager.Instance.enemyLayer);

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
					GameManager.Instance.obstructionLayer))
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
			nearestEnemy.GetComponent<IPoisonable>().StartPoisonEffect(poisonDuration, poisonDamage);
		}
	}

    public override void EndAbility(GameObject parent)
    {
		//Debug.Log("Poisonous Talons ability ended");
		nearestEnemy = null;
	}
}
