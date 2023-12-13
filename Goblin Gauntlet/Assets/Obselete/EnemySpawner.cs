using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject prefabToSpawn;	// Assign your prefab in the Inspector
	public float spawnInterval = 1f;	// Time between each spawn
	public float initialDelay = 0f; // Initial delay before the first spawn
	public int spawnAmount = 10;

	private float timer;	// Timer to keep track of time between spawns

	// Start is called before the first frame
	void Start()
	{
		timer = -initialDelay;	// Set the timer to negative initial delay
	}

	// Update is called once per frame
	void Update()
    {
		timer += Time.deltaTime;    // Increment the timer by the time since the last frame

		if (spawnAmount != 0)
		{
			if (timer > spawnInterval)  // If enough time has passed
			{
				Instantiate(prefabToSpawn, transform.position, Quaternion.identity);    // Spawn the prefab
				spawnAmount -= 1;
				timer = 0;  // Reset the timer
			}
		}
	}
}
