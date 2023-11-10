using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Poisonous Talons", menuName = "Scriptable Object/Ability/Poisonious Talons")]
public class PoisonousTalons : Ability
{
    public override void UseAbility(GameObject parent)
    {
        Debug.Log("Poisonous Talons ability used");
        // Activate a poison buff to attacks - when enemies are hit while this ability is active they will be dealt additional damage
    }
}
