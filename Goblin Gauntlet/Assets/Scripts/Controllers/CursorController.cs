using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
	private PlayerInput playerInput;
	private Player player;

	private GameObject playerCursor;

	private MultiplayerEventSystem multiplayerEventSystem;
	private GameObject currentSelectedGameObject;

	// Awake is called before Start
	void Awake()
	{
		playerInput = this.GetComponent<PlayerInput>();
		player = PlayerManager.Instance.FindPlayerByIndex(playerInput.playerIndex);

		playerCursor = this.GetComponentInChildren<RectTransform>().gameObject;

		multiplayerEventSystem = this.GetComponent<MultiplayerEventSystem>();
	}

	// Start is called before the first frame update
	void Start()
	{
		currentSelectedGameObject = multiplayerEventSystem.currentSelectedGameObject;
		playerCursor.GetComponent<RectTransform>().anchoredPosition = currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		//Debug.Log($"P{player.id + 1} multiplayerEventSystem.currentSelectedGameObject: {multiplayerEventSystem.currentSelectedGameObject}");

		string loadCursorImage = $"Sprites/PlayerCursors/Player{player.id + 1}Cursor";
		playerCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(loadCursorImage);

		playerCursor.GetComponent<RectTransform>().anchoredPosition = currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		playerCursor.GetComponent<RectTransform>().sizeDelta = currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta;
		playerCursor.GetComponent<RectTransform>().anchorMin = currentSelectedGameObject.GetComponent<RectTransform>().anchorMin;
		playerCursor.GetComponent<RectTransform>().anchorMax = currentSelectedGameObject.GetComponent<RectTransform>().anchorMax;
		playerCursor.GetComponent<RectTransform>().pivot = currentSelectedGameObject.GetComponent<RectTransform>().pivot;
		playerCursor.GetComponent<RectTransform>().rotation = currentSelectedGameObject.GetComponent<RectTransform>().rotation;
		playerCursor.GetComponent<RectTransform>().localScale = currentSelectedGameObject.GetComponent<RectTransform>().localScale;
	}

	// Update is called once per frame
	void Update()
    {
		currentSelectedGameObject = multiplayerEventSystem.currentSelectedGameObject;
		Debug.Log($"P{player.id + 1} currentSelectedGameObject: {currentSelectedGameObject.name}");

		if (currentSelectedGameObject != null)
		{
			playerCursor.GetComponent<RectTransform>().anchoredPosition = currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		}
	}

	public void OnCharacterButtonClick()
	{
		Debug.Log($"{currentSelectedGameObject.name} was clicked by P{player.id + 1}");
	}

	public void OnReadyUpButtonClick()
	{
	}
}
