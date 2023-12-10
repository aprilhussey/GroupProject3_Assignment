using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackRadiusCollider : MonoBehaviour
{
	private GoblinController goblinController;

	// Awake is called before Start
	void Awake()
	{
		// Get reference to GoblinController.cs on parent game object
		goblinController = GetComponentInParent<GoblinController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (goblinController != null)
		{
			goblinController.AttackRadiusEntered(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (goblinController != null)
		{
			goblinController.AttackRadiusExited(other.gameObject);
		}
}
}
