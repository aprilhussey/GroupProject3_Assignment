using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
	private GoblinController goblinController;

	void Start()
	{
		goblinController = GetComponentInParent<GoblinController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == goblinController.player)
		{
			goblinController.AttackPlayer();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == goblinController.player)
		{
			goblinController.StopAttackingPlayer();
		}
	}
}
