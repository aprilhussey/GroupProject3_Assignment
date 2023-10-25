using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : MonoBehaviour
{
    public float fieldOfView = 45f;

    public SphereCollider followRadiusCollider;   // Reference to the 'follow radius' game object collider
    private float visionDistance;

    private GoblinControllerOld goblinController;

    void Start()
    {
        goblinController = GetComponentInParent<GoblinControllerOld>();
    }

    // Update is called once per frame
    void Update()
    {
        visionDistance = followRadiusCollider.radius;   // Use the sphere collider's radius as the vision distance

        Collider[] playersInViewRadius = Physics.OverlapSphere(transform.position, visionDistance, goblinController.playerLayer);

        foreach (Collider playerCollider in playersInViewRadius)
        {
            Vector3 directionToPlayer = (playerCollider.transform.position - transform.position).normalized;
            
            // Check if player is within the field of view
            if (Vector3.Angle(transform.forward, directionToPlayer) < fieldOfView / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);

                // Check if there are obstructions between the AI and the player
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, goblinController.obstructionLayer))
                {
                    //Debug.Log("Player detected");
                    GameObject player = playerCollider.gameObject;
                    /*if (!goblinController.characterData.playersSeen.Contains(player))
                    {
						Debug.Log("New Player detected");
						goblinController.characterData.playersSeen.Add(player);
                    }*/
                    //FollowPlayer(player)
                }
            }
        }
    }

    // Draw gizmo in editor
    void OnDrawGizmos()
    {
		visionDistance = followRadiusCollider.radius;   // Use the sphere collider's radius as the vision distance

		// Draw field of view
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * visionDistance);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * visionDistance);

		// Draw vision distance
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, visionDistance);
	}
}
