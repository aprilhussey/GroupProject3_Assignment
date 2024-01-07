using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
	// Singleton instance
	public static CoroutineStarter Instance = null;

	void Awake()
	{
		// Ensure only one GameManager instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy GameManager when switching scenes
		DontDestroyOnLoad(gameObject);
	}
}
