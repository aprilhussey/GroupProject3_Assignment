using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
	// Singleton instance
	public static PlayerSpawnManager Instance = null;

	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

	public GameObject[] spawnLocations;

	public GameObject[] playerPrefabs;

	private Player player;

	// Awake is called before Start
	private void Awake()
	{
		// Ensure only one PlayerSpawnManager instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		playerInputManager = GetComponent<PlayerInputManager>();
		maxPlayerCount = playerInputManager.maxPlayerCount;
	}

    public void OnPlayerJoined(PlayerInput playerInput)
    {
		/*Debug.Log("PlayerInput ID: " + playerInput.playerIndex);
        //Sets the player ID starting from 1 (adds 1 as it is usually 0 based
        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;
        //Sets the spawn position of the player based on the array of spawn points
        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].position;*/
		
		if (PlayerManager.Instance.players.Count < maxPlayerCount)  // If the amount of players in the player manager is less than the max player count
		{
			// Spawn pf_CharacterSelectPlayerMenu
			// Initialize new player in PlayerManager

			PlayerManager.Instance.Initialize(playerInput.playerIndex, playerInput);

			//playerInput.gameObject.transform.position = spawnLocations[playerInput.playerIndex].transform.positiont

			playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex;
            //playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].transform.position;

            playerInputManager.playerPrefab = DeterminePrefab(playerInput);
		}
		else
		{
			// Max amount of players already joined
			Debug.LogWarning($"WARNING: New player tried to join when there are already the max amount of players");
		}
	}

	private GameObject DeterminePrefab(PlayerInput playerInput)
	{
		player = PlayerManager.Instance.FindPlayerByIndex(playerInput.playerIndex);
		
		if (player.id == 0)
		{
			// Paladin
			return playerPrefabs[0];
		}
		else if (player.id == 1)
		{
			// Rogue
			return playerPrefabs[1];
		}
		else if (player.id == 2)
		{
			// Warlock
			return playerPrefabs[2];
		}
		else if (player.id == 3)
		{
			// Cleric
			return playerPrefabs[3];
		}
		else
		{
			return null;
		}
	}
}
