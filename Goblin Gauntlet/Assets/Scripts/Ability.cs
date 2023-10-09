using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public string id;
    public string name;
    public int damage;
    public float cooldown;

    public void UseAbility(Character target)
    {
        target.TakeDamage(damage);
        // Add code to start cooldown
    }
}
