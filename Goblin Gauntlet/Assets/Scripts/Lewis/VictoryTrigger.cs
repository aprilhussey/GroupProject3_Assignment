using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
	public GameObject victoryScreen;


	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Collider Collided");
			victoryScreen.SetActive(true);
		}
	}

}
