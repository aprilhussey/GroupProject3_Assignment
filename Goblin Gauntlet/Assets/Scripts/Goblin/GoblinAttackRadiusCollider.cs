using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackRadiusCollider : MonoBehaviour
{
	private GoblinController goblinController;

	// Start is called before the first frame update
	void Start()
	{
		// Get reference to GoblinCOntroller.cs on parent game object
		goblinController = GetComponentInParent<GoblinController>();
	}

	void OnTriggerEnter(Collider other)
	{
		goblinController.AttackRadiusEntered(other.gameObject);
	}

	void OnTriggerExit(Collider other)
	{
		goblinController.AttackRadiusExited(other.gameObject);
	}
}
