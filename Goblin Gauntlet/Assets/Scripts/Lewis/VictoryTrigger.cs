using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
	public GameObject victoryScreen;


	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "Island")
		{
			victoryScreen.SetActive(true);
		}
	}

}
