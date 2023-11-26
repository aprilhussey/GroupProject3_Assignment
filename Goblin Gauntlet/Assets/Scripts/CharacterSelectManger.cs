using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
	private PlayerInputManager playerInputManager;
	private int maxPlayerCount;

	private GameObject characterButtonsParent;
	private GameObject playerCursorsParent;

	List<GameObject> characterButtons;
	List<GameObject> playerCursors;

	private void Awake()
	{
		playerInputManager = GetComponent<PlayerInputManager>();
		maxPlayerCount = playerInputManager.maxPlayerCount;

		characterButtonsParent = GameObject.Find("CharacterButtons");
		playerCursorsParent = GameObject.Find("PlayerCursors");

		characterButtons = new List<GameObject>();
		playerCursors = new List<GameObject>();
	}

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
