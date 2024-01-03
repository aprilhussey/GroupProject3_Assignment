using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class PlayerManager : MonoBehaviour
{
	// Singleton instance
	public static PlayerManager Instance = null;

	public List<Player> players;

	private List<GameObject> characterPrefabs;

	void Awake()
	{
		// Ensure only one PlayerManager instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy PlayerManager when switching scenes
		DontDestroyOnLoad(gameObject);

		players = new List<Player>();
	}

	// Initialize your player here
	public void Initialize(int index, PlayerInput input)
	{
		int id = players.Count;
		Player player = new Player(id, index, input);
		players.Add(player);
	}

	public Player FindPlayerByIndex(int index)
	{
		foreach (Player player in players)
		{
			if (player.index == index)
			{
				return player;
			}
		}
		return null;
	}

	public void SetCharacterPrefab(int id)
	{
		Player player = players[id];

		characterPrefabs = CharacterSelectManager.Instance.characterPrefabs;

        if (player != null)
		{
			foreach (GameObject characterPrefab in characterPrefabs)
			{
				if (player.character == characterPrefab.GetComponent<PlayerController>().characterData)
				{
					player.characterPrefab = characterPrefab;
				}
				else
				{
					// Do nothing
					break;
				}
			}
		}
		else
		{
			Debug.Log($"player is null");
		}

	}

	public void SetPlayerInputData(ref PlayerInput playerInput)
	{
		Player player = players[playerInput.playerIndex];

		// Switch the control scheme of playerInput to the device
		playerInput.SwitchCurrentControlScheme(player.controlScheme, player.devices);
	}

	public void SetPlayerMaxHealthOnHealthBar(ref PlayerInput playerInput, float maxHealth)
	{
		HealthBar playerHealthBar = GameObject.Find($"P{playerInput.playerIndex + 1}HealthBar").GetComponent<HealthBar>();
			
		playerHealthBar.SetMaxHealth(maxHealth);
	}

	public void UpdatePlayerHealthBar(ref PlayerInput playerInput, float health)
	{
		HealthBar playerHealthBar = GameObject.Find($"P{playerInput.playerIndex + 1}HealthBar").GetComponent<HealthBar>();
		playerHealthBar.SetHealth(health);
	}
}