using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollowRadiusCollider : MonoBehaviour
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
        goblinController.FollowRadiusEntered(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        goblinController.FollowRadiusExited(other.gameObject);
    }
}
