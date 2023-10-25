using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollow : MonoBehaviour
{
	private GoblinControllerOld goblinController;

	void Start()
	{
		goblinController = GetComponentInParent<GoblinControllerOld>();
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Start following the player");
		if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			/*
			// If no target or new target is closer
			if (goblinController.characterData.closestPlayer == null ||
				Vector3.Distance(goblinController.gameObject.transform.position, other.gameObject.transform.position)
				< Vector3.Distance(goblinController.transform.position, goblinController.characterData.closestPlayer.transform.position))
			{
				goblinController.characterData.closestPlayer = other.gameObject;
			}
			*/
		}
	}

	void OnTriggerStay(Collider other)
	{
		Debug.Log("Following the player");
		if (goblinController.characterData.closestPlayer == other.gameObject)
		{
			if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
			{
				GameObject player = other.gameObject;
				goblinController.FollowPlayer(player);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			GameObject player = other.gameObject;
			goblinController.StopFollowingPlayer(player);
		}
	}
}
