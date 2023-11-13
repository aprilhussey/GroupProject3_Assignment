using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Attack", menuName = "Scriptable Object/Ability/Basic Attack")]
public class BasicAttack : Ability
{
	private PlayerController playerController;
	public float damage = 1f;

	public override void UseAbility(GameObject parent)
	{
		playerController = parent.GetComponent<PlayerController>();

		Debug.Log("Basic Attack used");
		playerController.damage = damage;
	}

	public override void AbilityActive(GameObject parent, AbilityState abilityState) 
	{
		if (abilityState == AbilityState.ready)
		{

		}
	}

	public override void EndAbility(GameObject parent)
	{
		playerController = parent.GetComponent<PlayerController>();

		Debug.Log("Basic Attack ended");
	}
}
