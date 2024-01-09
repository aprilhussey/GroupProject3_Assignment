using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	// Singleton instance
	public static DontDestroy Instance = null;

	private void Awake()
	{
		// Ensure only one instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy when switching scenes
		DontDestroyOnLoad(this.gameObject);
	}
}
