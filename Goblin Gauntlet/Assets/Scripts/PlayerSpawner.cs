using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
	private List<Player> players = new List<Player>();

	[SerializeField]
	private List<GameObject> spawnLocations = new List<GameObject>();

	private void Awake()
	{
		players = PlayerManager.Instance.players;

		if (players != null)
		{
			foreach (Player player in players)
			{
				// Instantiate th echaracter prefab at the spawn location
				GameObject character = Instantiate(player.characterPrefab, spawnLocations[player.id].transform.position, Quaternion.identity);

				PlayerInput characterInput = character.GetComponent<PlayerInput>();

				PlayerManager.Instance.SetPlayerInputData(ref characterInput);
			}
		}
	}
}
