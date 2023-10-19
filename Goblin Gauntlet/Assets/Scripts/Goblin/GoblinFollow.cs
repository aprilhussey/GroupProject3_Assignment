using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollow : MonoBehaviour
{
	private GoblinController goblinController;
	private float speed = 2.0f;
	private float rotationSpeed = 5.0f;

	void Start()
	{
		goblinController = GetComponentInParent<GoblinController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (goblinController.characterData.playersSeen.Contains(other.gameObject))
		{
			if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
			{
				GameObject player = other.gameObject;
				//goblinController.FollowPlayer(player);
				//goblinController.gameObject.transform.position = Vector3.MoveTowards(goblinController.gameObject.transform.position, player.transform.position, speed * Time.deltaTime);

				Vector3 targetDirection = (other.gameObject.transform.position - transform.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

				goblinController.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
				goblinController.gameObject.transform.position = Vector3.MoveTowards(transform.position, other.gameObject.transform.position, speed * Time.deltaTime);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (goblinController.characterData.playersSeen.Contains(other.gameObject))
		{
			if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
			{
				GameObject player = other.gameObject;
				//goblinController.FollowPlayer(player);
				//goblinController.gameObject.transform.position = Vector3.MoveTowards(goblinController.gameObject.transform.position, player.transform.position, speed * Time.deltaTime);

				Vector3 targetDirection = (other.gameObject.transform.position - transform.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

				goblinController.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
				goblinController.gameObject.transform.position = Vector3.MoveTowards(transform.position, other.gameObject.transform.position, speed * Time.deltaTime);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if ((goblinController.playerLayer.value & (1 << other.gameObject.layer)) != 0)
		{
			GameObject player = other.gameObject;
			goblinController.StopFollowingPlayer(player);
		}
	}
}
