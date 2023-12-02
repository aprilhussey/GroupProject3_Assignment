using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
	// Singleton instance
	public static CharacterSelectManager Instance = null;

	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

	private GameObject characterButtonsParent;
	private GameObject playerCursorsParent;

	public List<GameObject> characterButtons;
	private List<GameObject> playerCursors;

	private Player playerReciever;
	private PlayableCharacter characterReciever;

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

		characterButtonsParent = GameObject.Find("CharacterButtons");
		playerCursorsParent = GameObject.Find("PlayerCursors");

		characterButtons = new List<GameObject>();
		playerCursors = new List<GameObject>();
	}

	// Start is called before the first frame update
	private void Start()
	{
		for (int i = 0; i < characterButtonsParent.transform.childCount; i++)
		{
			Transform childTransform = characterButtonsParent.transform.GetChild(i);
			characterButtons.Add(childTransform.gameObject);
		}
	}

	public void OnPlayerJoined(PlayerInput playerInput)
	{
		if (playerCursors.Count != maxPlayerCount)
		{
			// Add Player
			GameObject newPlayerCursor = playerInput.gameObject;
			if (playerCursorsParent != null)
			{
				newPlayerCursor.transform.SetParent(playerCursorsParent.transform);

				string loadString = $"Sprites/PlayerCursors/Player{playerCursors.Count + 1}Cursor";

				newPlayerCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(loadString);

				newPlayerCursor.GetComponent<RectTransform>().anchoredPosition = characterButtons[0].GetComponent<RectTransform>().anchoredPosition;
				newPlayerCursor.GetComponent<RectTransform>().sizeDelta = characterButtons[0].GetComponent<RectTransform>().sizeDelta;
				newPlayerCursor.GetComponent<RectTransform>().anchorMin = characterButtons[0].GetComponent<RectTransform>().anchorMin;
				newPlayerCursor.GetComponent<RectTransform>().anchorMax = characterButtons[0].GetComponent<RectTransform>().anchorMax;
				newPlayerCursor.GetComponent<RectTransform>().pivot = characterButtons[0].GetComponent<RectTransform>().pivot;
				newPlayerCursor.GetComponent<RectTransform>().rotation = characterButtons[0].GetComponent<RectTransform>().rotation;
				newPlayerCursor.GetComponent<RectTransform>().localScale = characterButtons[0].GetComponent<RectTransform>().localScale;

				// Assign an ID to the player
				PlayerManager.Instance.Initialize(playerInput.playerIndex, playerInput);

				playerCursors.Add(newPlayerCursor);
			}
		} 
		else
		{
			Debug.LogWarning($"WARNING: New player tried to join when there are already the max amount of players");
		}
	}

	public void OnCharacterButtonClicked(Player playerRecieved, PlayableCharacter playableCharacterRecieved)
	{
		playerReciever = playerRecieved;
		characterReciever = playableCharacterRecieved;
		Debug.Log($"player.id: {playerReciever.id}");

		Player player = PlayerManager.Instance.players[playerReciever.id];

		player.character = characterReciever;
		Debug.Log($"player.character: {player.character}");

		string pathToReadyParent = $"PlayerCharacters/P{player.id + 1}_Character/P{player.id + 1}_Ready";
		GameObject readyParent = GameObject.Find(pathToReadyParent);

		string pathToReadyUpButton = $"PlayerCharacters/P{player.id + 1}_Character/P{player.id + 1}_Ready/btn_ReadyUp";
		GameObject readyUpButton = GameObject.Find(pathToReadyUpButton);

		Debug.Log($"readyParent: {readyParent.name}");

		readyParent.SetActive(true);
		
		MultiplayerEventSystem multiplayerEventSystem = player.input.gameObject.GetComponent<MultiplayerEventSystem>();
		multiplayerEventSystem.SetSelectedGameObject(readyUpButton);

		Image playerCursorImage = player.input.gameObject.GetComponent<Image>();
		playerCursorImage.enabled = false;
	}

	public void OnReadyUpClick()
	{
		Player player = PlayerManager.Instance.players[playerReciever.id];

		string pathToReadyUpButton = $"PlayerCharacters/P{player.id + 1}_Character/P{player.id + 1}_Ready/btn_ReadyUp";
		GameObject readyUpButton = GameObject.Find(pathToReadyUpButton);

		readyUpButton.SetActive(false);

		MultiplayerEventSystem multiplayerEventSystem = player.input.gameObject.GetComponent<MultiplayerEventSystem>();
		multiplayerEventSystem.SetSelectedGameObject(null);

		// Set player.isReady to true
		player.isReady = true;
	}
}
