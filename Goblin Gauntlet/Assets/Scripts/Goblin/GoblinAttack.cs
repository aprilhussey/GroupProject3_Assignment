using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
	private GoblinControllerOld goblinController;

	void Start()
	{
		goblinController = GetComponentInParent<GoblinControllerOld>();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			GameObject player = other.gameObject;
			goblinController.AttackPlayer(player);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			GameObject player = other.gameObject;
			goblinController.StopAttackingPlayer(player);
		}
	}
}
