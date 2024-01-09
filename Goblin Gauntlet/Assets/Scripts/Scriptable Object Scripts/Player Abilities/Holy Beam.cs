using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
        Vector3 playerPos = playerController.transform.position;
        Vector3 playerDirection = playerController.transform.forward;
        Quaternion playerRotation = playerController.transform.rotation;
        float spawnDistance = 10;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        childObject = Instantiate(paladinBeam, spawnPos, playerRotation * Quaternion.Euler(270,0,0));
    }

    public override void EndAbility(GameObject parent)
    {
        playerController = parent.GetComponent<PlayerController>();
		Destroy(childObject);
		//Debug.Log("Holy Beam ability ended");
	}
}
