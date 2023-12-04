using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

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

	private void Update()
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

                // Check if the target is already in the target group
                if (!targetGroup.m_Targets.Any(t => t.target == target))
                {
                    // Define the weight and radius
                    float weight = 1.0f;
                    float radius = 1.0f;

                    // Add the game object to the target group
                    targetGroup.AddMember(target, weight, radius);
                }
			}
		}
	}
}
