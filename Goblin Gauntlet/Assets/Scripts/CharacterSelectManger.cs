using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
	// Singleton instance
	public static CharacterSelectManager Instance = null;

	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

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

		playerInputManager = GetComponent<PlayerInputManager>();
		maxPlayerCount = playerInputManager.maxPlayerCount;
	}

	void Update()
	{
        if (PlayerManager.Instance.players.Count == maxPlayerCount)
        {
			if (AreAllPlayersReady())
			{
				// Load the first level
				Debug.Log($"All players are ready");
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
}
