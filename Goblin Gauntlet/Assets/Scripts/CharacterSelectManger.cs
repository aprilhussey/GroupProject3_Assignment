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

	void Start()
	{
		/*Image canvasP1TitleImage = GameObject.Find($"MainCanvas/PlayerCharacters/P1_Character").GetComponent<Image>();
		string loadCanvasP1TitleImageGray = $"Sprites/PlayerTitles/Player1TitleGray";
		canvasP1TitleImage.sprite = Resources.Load<Sprite>(loadCanvasP1TitleImageGray);

		Image canvasP2TitleImage = GameObject.Find($"MainCanvas/PlayerCharacters/P2_Character").GetComponent<Image>();
		string loadCanvasP2TitleImageGray = $"Sprites/PlayerTitles/Player2TitleGray";
		canvasP2TitleImage.sprite = Resources.Load<Sprite>(loadCanvasP2TitleImageGray);

		Image canvasP3TitleImage = GameObject.Find($"MainCanvas/PlayerCharacters/P3_Character").GetComponent<Image>();
		string loadCanvasP3TitleImageGray = $"Sprites/PlayerTitles/Player3TitleGray";
		canvasP3TitleImage.sprite = Resources.Load<Sprite>(loadCanvasP3TitleImageGray);

		Image canvasP4TitleImage = GameObject.Find($"MainCanvas/PlayerCharacters/P4_Character").GetComponent<Image>();
		string loadCanvasP4TitleImageGray = $"Sprites/PlayerTitles/Player4TitleGray";
		canvasP4TitleImage.sprite = Resources.Load<Sprite>(loadCanvasP4TitleImageGray);*/
	}

	void Update()
	{
        if (PlayerManager.Instance.players.Count == maxPlayerCount)
        {
			if (AreAllPlayersReady())
			{
				// Load the first level
				Debug.Log($"All players are ready");
				GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
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
