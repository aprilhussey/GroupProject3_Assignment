using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Poisonous Talons", menuName = "Scriptable Object/Ability/Poisonious Talons")]
public class PoisonousTalons : Ability
{
    private PlayerController playerController;
    public float damageBuff;

    public override void UseAbility(GameObject parent)
    {
		playerController = parent.GetComponent<PlayerController>();

		Debug.Log("Poisonous Talons ability used");
        playerController.damage += damageBuff;
    }

    public override void EndAbility(GameObject parent)
    {
		playerController = parent.GetComponent<PlayerController>();

		Debug.Log("Poisonous Talons ability ended");
        playerController.damage -= damageBuff;
    }
}
