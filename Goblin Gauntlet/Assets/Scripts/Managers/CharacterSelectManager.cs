using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
	// Singleton instance
	public static CharacterSelectManager Instance = null;

	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

	public List<GameObject> characterPrefabs = new List<GameObject>();

	public List<GameObject> characterSelectPrefabs = new List<GameObject>();

	// Awake is called before Start
	private void Awake()
	{
		// Ensure only one CharacterSelectManager instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		playerInputManager = this.GetComponent<PlayerInputManager>();
		maxPlayerCount = playerInputManager.maxPlayerCount;
	}

	private float delayTime = 3f;
	private float timer = 0f;

	void Update()
	{
        if (PlayerManager.Instance.players.Count == maxPlayerCount)
        {
			if (AreAllPlayersReady())
			{
				timer += Time.deltaTime;
				//Debug.Log($"timer {timer}");
				if (timer > delayTime)
				{
					// All players are ready
					// Delay time has passed
					timer = 0;

					// Load the first level
					Debug.Log($"All players are ready");
					GameManager.Instance.ChangeGameState(GameManager.GameState.Level001);
				}
			}
			else
			{
				timer = 0;
			}
		}
	}

	public void OnPlayerJoined(PlayerInput playerInput)
	{
		if (PlayerManager.Instance.players.Count < maxPlayerCount)	// If the amount of players in the player manager is less than the max player count
		{
			// Spawn pf_CharacterSelectPlayerMenu
			// Initialize new player in PlayerManager
			PlayerManager.Instance.Initialize(playerInput.playerIndex, playerInput);
		}
		else
		{
			// Max amount of players already joined
			Debug.LogWarning($"WARNING: New player tried to join when there are already the max amount of players");
		}
	}

	private bool AreAllPlayersReady()
	{
		foreach (Player player in PlayerManager.Instance.players)
		{
			// If any player is not ready, return false
			if (!player.isReady)
			{
				return false;
			}
		}
		// All players are ready
		return true;
	}

	public void SpawnCharacterPrefab(int playerID, PlayableCharacter characterData)
	{
		Vector3 spawnLocation = new Vector3();
		Quaternion spawnRotation = new Quaternion();

		switch (playerID)
		{
			case 0:
				// Player 1
				spawnLocation = new Vector3(-160, 0, 300);
				spawnRotation = Quaternion.Euler(0, 145, 0);
				break;
			case 1:
				// Player 2
				spawnLocation = new Vector3(-45, 0, 300);
				spawnRotation = Quaternion.Euler(0, 165, 0);
				break;
			case 2:
				// Player 3
				spawnLocation = new Vector3(70, 0, 300);
				spawnRotation = Quaternion.Euler(0, 195, 0);
				break;
			case 3:
				// Player 4
				spawnLocation = new Vector3(185, 0, 300);
				spawnRotation = Quaternion.Euler(0, 210, 0);
				break;
		}

		if (characterSelectPrefabs != null)
		{
			foreach (GameObject characterSelectPrefab in characterSelectPrefabs)
			{
				PlayableCharacter characterPrefabData = characterSelectPrefab.GetComponent<PlayableCharacterHolder>().playableCharacter;
				if (characterData == characterPrefabData)
				{
					Instantiate(characterSelectPrefab, spawnLocation, spawnRotation);

					// Change the scale
					//instantiatedPrefab.transform.localScale = new Vector3(27.9f, 27.9f, 27.9f);
				}
			}
		}
	}
}
