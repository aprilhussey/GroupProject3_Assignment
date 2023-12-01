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
}

public class Player
{
	public int id;
	public int index;
	public PlayerInput input;
	public bool isReady;
	public PlayableCharacter character;

	public Player(int id, int index, PlayerInput input)
	{
		this.id = id;
		this.input = input;
		this.index = index;
		isReady = false;
		character = null;
	}
}