using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollowRadiusCollider : MonoBehaviour
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
            goblinController.FollowRadiusEntered(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (goblinController != null)
        {
            goblinController.FollowRadiusExited(other.gameObject);
        }
    }
}
