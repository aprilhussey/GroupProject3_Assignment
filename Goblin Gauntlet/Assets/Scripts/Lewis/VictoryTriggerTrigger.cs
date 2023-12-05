using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTriggerTrigger : MonoBehaviour
{
    public GameObject victoryTrigger;

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Artifact"))
		{
			Debug.Log("Collider Left");
			victoryTrigger.SetActive(true);
		}
	}
}
