using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Holy Beam", menuName = "Scriptable Object/Ability/Holy Beam")]
public class HolyBeam : Ability
{
    private PlayerController playerController;
    public GameObject paladinBeam;
    public GameObject childObject;

    public override void UseAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

        Debug.Log("Holy Beam ability used");
        childObject = Instantiate(paladinBeam) as GameObject;
        childObject.transform.parent = playerController.transform;
        childObject.transform.position = playerController.transform.position + new Vector3(0, 1.5f, 10.5f);
    }

    public override void EndAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();

        Debug.Log("Holy Beam ability ended");
        Destroy(childObject);
    }
}
