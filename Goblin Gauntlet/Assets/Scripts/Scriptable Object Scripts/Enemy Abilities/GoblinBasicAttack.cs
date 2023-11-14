using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Goblin Basic Attack", menuName = "Scriptable Object/Ability/Goblin Basic Attack")]
public class GoblinBasicAttack : Ability
{
	private GoblinController goblinController;

	public float damage = 1f;

	public override void UseAbility(GameObject parent)
	{
		goblinController = parent.GetComponent<GoblinController>();

		if (goblinController.target != null)
		{
			IDamageable damageable = goblinController.target.GetComponent<IDamageable>();
			if (damageable != null)
			{
				Debug.Log($"damageable = {damageable} ");
				damageable.TakeDamage(damage);
				Debug.Log($"{parent.name} basic attack used");
			}
		}
	}

	public override void EndAbility(GameObject parent)
	{
		Debug.Log($"{parent.name} basic attack ended");
	}
}
