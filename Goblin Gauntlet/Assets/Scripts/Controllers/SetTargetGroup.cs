using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetTargetGroup : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;
    private LayerMask playerLayer;

    // Awake is called before Start
    void Awake()
    {
		targetGroup = GetComponent<CinemachineTargetGroup>();
		playerLayer = LayerMask.GetMask("Player");
	}

    // Start is called before the first frame update
    void Start()
    {
        // Get all game objects on the Player layer
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Loop through each game object
        foreach (GameObject obj in allObjects)
        {
            // Check if the GameObject is on playerLayer
            if (playerLayer == (playerLayer | (1 << obj.layer)))
            {
                // Get the transform component
                Transform target = obj.transform;

                // Define the weight and radius
                float weight = 1.0f;
                float radius = 1.0f;

                // Add the game object to the target group
                targetGroup.AddMember(target, weight, radius);
            }
        }
    }
}
