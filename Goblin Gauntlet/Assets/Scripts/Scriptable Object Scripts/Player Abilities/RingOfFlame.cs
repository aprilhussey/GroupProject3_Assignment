using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Ring of Flame", menuName = "Scriptable Object/Ability/Ring of Flame")]
public class RingOfFlame : Ability
{
    private PlayerController playerController;
    public GameObject warlockReticle;
    public GameObject childObject;

	public override void UseAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

		//this.attacking = true;


		//Debug.Log("Ring of Flame ability used");
        playerController.canMove = false;
        childObject = Instantiate(warlockReticle, playerController.transform.position, Quaternion.identity, playerController.transform);
        //CoroutineStarter.Instance.StartCoroutine(AnimDelay());

    }

    public override void EndAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

        //this.attacking = false;
        //Debug.Log("Ring of Flame ability ended");
        playerController.canMove = true;
        Destroy(childObject);
    }

    IEnumerator AnimDelay()
    {
        this.attacking = true;
        yield return new WaitForSeconds(1);
        this.attacking = false;
    }
}