using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
	// Singleton instance
	public static CharacterSelectManager instance = null;

	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

	private GameObject characterButtonsParent;
	private GameObject playerCursorsParent;

	public List<GameObject> characterButtons;
	private List<GameObject> playerCursors;

	// Awake is called before Start
	private void Awake()
	{
		// Ensure only one CharacterSelectManager instance exists
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
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

				playerCursors.Add(newPlayerCursor);
			}
		} 
		else
		{
			Debug.LogWarning($"WARNING: New player tried to join when there are already the max amount of players");
		}
	}
}
