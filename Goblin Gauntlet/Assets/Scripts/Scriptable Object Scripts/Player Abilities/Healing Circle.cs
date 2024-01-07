using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Circle", menuName = "Scriptable Object/Ability/Healing Circle")]
public class HealingCircle : Ability
{
    private PlayerController playerController;
    public GameObject clericHeal;
    public GameObject childObject;

    public override void UseAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

		this.attacking = true;

		Debug.Log("Healing Circle ability used");
        childObject = Instantiate(clericHeal) as GameObject;
        childObject.transform.parent = playerController.transform;
        childObject.transform.position = playerController.transform.position;
    }

    public override void EndAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

		this.attacking = false;

		Debug.Log("Healing Circle ability ended");
        Destroy(childObject);
    }
}
