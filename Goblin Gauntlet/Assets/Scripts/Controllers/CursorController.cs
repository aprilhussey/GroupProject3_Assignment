using System.Collections;
using System.Collections.Generic;
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

	private GameObject readyParent;
	private GameObject readyUpButton;

	private GameObject mainCanvas;
	private GameObject canvasCursor;

	private MultiplayerEventSystem canvasMultiplayerEventSystem;
	private GameObject canvasCurrentSelectedGameObject;

	private GameObject canvasReadyParent;
	private GameObject canvasReadyUpButton;

	private Image canvasPlayerTitleImage;

	// Awake is called before Start
	void Awake()
	{
		playerInput = this.GetComponent<PlayerInput>();
		player = PlayerManager.Instance.FindPlayerByIndex(playerInput.playerIndex);

		playerCursor = this.GetComponentInChildren<RectTransform>().gameObject;

		multiplayerEventSystem = this.GetComponent<MultiplayerEventSystem>();

		// Set canvas variables
		mainCanvas = GameObject.Find("MainCanvas");
		canvasCursor = GameObject.Find($"MainCanvas/PlayerCursors/P{player.id + 1}_Cursor");
		canvasCursor.SetActive(true);

		canvasMultiplayerEventSystem = canvasCursor.GetComponent<MultiplayerEventSystem>();
		canvasMultiplayerEventSystem.SetSelectedGameObject(GameObject.Find($"MainCanvas/CharacterButtons/PaladinAzreal_CharacterButton"));

		canvasReadyParent = GameObject.Find($"MainCanvas/PlayerCharacters/P{player.id + 1}_Character/P{player.id + 1}_Ready");
		canvasReadyUpButton = GameObject.Find($"MainCanvas/PlayerCharacters/P{player.id + 1}_Character/P{player.id + 1}_Ready/btn_ReadyUp");

		canvasPlayerTitleImage = GameObject.Find($"MainCanvas/PlayerCharacters/P{player.id + 1}_Character").GetComponent<Image>();
	}

	// Start is called before the first frame update
	void Start()
	{
		currentSelectedGameObject = multiplayerEventSystem.currentSelectedGameObject;
		SetCursor(playerCursor, currentSelectedGameObject);

		GameObject proxyCanvas = this.transform.parent.gameObject;
		readyParent = proxyCanvas.transform.Find("PlayerCharacters").gameObject.transform.Find($"P{player.id + 1}_Character").gameObject.transform.Find($"P{player.id + 1}_Ready").gameObject;
		readyUpButton = readyParent.transform.Find("btn_ReadyUp").gameObject;

		// Set canvas varaibles
		canvasCurrentSelectedGameObject = canvasMultiplayerEventSystem.currentSelectedGameObject;
		SetCursor(canvasCursor, canvasCurrentSelectedGameObject);

		string loadCanvasButtonBackgroundColor = $"Sprites/Buttons/ButtonBackgroundColor";
		canvasPlayerTitleImage.sprite = Resources.Load<Sprite>(loadCanvasButtonBackgroundColor);
	}

	// Update is called once per frame
	void Update()
    {
		currentSelectedGameObject = multiplayerEventSystem.currentSelectedGameObject;

		if (currentSelectedGameObject != null)
		{
			Debug.Log($"P{player.id + 1} currentSelectedGameObject: {currentSelectedGameObject.name}");
			
			playerCursor.GetComponent<RectTransform>().anchoredPosition = currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		}

		// Set canvas variables
		canvasCurrentSelectedGameObject = canvasMultiplayerEventSystem.currentSelectedGameObject;

		// Check for updates in proxy canvas vs main canvas
		if ((canvasCurrentSelectedGameObject != null && currentSelectedGameObject != null) && currentSelectedGameObject.name != canvasCurrentSelectedGameObject.name)
		{
			canvasMultiplayerEventSystem.SetSelectedGameObject(GameObject.Find($"MainCanvas/CharacterButtons/" + currentSelectedGameObject.name));
			canvasCurrentSelectedGameObject = canvasMultiplayerEventSystem.currentSelectedGameObject;
		}

		if (canvasCurrentSelectedGameObject != null)
		{
			Debug.Log($"P{player.id + 1} canvasCurrentSelectedGameObject: {canvasCurrentSelectedGameObject.name}");

			canvasCursor.GetComponent<RectTransform>().anchoredPosition = canvasCurrentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		}
	}

	public void OnCharacterButtonClick()
	{
		Debug.Log($"{currentSelectedGameObject.name} was clicked by P{player.id + 1}");

		PlayableCharacter character = currentSelectedGameObject.GetComponent<PlayableCharacterHolder>().playableCharacter;
		PlayerManager.Instance.players[player.id].character = character;

		Debug.Log($"readyParent: {readyParent.name}");
		readyParent.SetActive(true);

		multiplayerEventSystem.SetSelectedGameObject(readyUpButton);

		Image playerCursorImage = playerCursor.GetComponent<Image>();
		playerCursorImage.enabled = false;

		// Set canvas varaibles
		canvasReadyParent.SetActive(true);

		canvasMultiplayerEventSystem.SetSelectedGameObject(canvasReadyUpButton);

		Image canvasCursorImage = canvasCursor.GetComponent<Image>();
		canvasCursorImage.enabled = false;
	}

	public void OnReadyUpButtonClick()
	{
		readyUpButton.SetActive(false);

		multiplayerEventSystem.SetSelectedGameObject(null);

		PlayerManager.Instance.SetCharacterPrefab(player.id);
		PlayerManager.Instance.players[player.id].isReady = true;

		Debug.Log($"player.characterPrefab = {PlayerManager.Instance.players[player.id].characterPrefab.name}");

		// Set canvas variables
		canvasReadyUpButton.SetActive(false);

		canvasMultiplayerEventSystem.SetSelectedGameObject(null);
	}

	private void SetCursor(GameObject cursor, GameObject selectedGameObject)
	{
		cursor.GetComponent<RectTransform>().anchoredPosition = selectedGameObject.GetComponent<RectTransform>().anchoredPosition;

		string loadCursorImage = $"Sprites/PlayerCursors/Player{player.id + 1}Cursor";
		cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>(loadCursorImage);

		cursor.GetComponent<RectTransform>().anchoredPosition = selectedGameObject.GetComponent<RectTransform>().anchoredPosition;
		cursor.GetComponent<RectTransform>().sizeDelta = selectedGameObject.GetComponent<RectTransform>().sizeDelta;
		cursor.GetComponent<RectTransform>().anchorMin = selectedGameObject.GetComponent<RectTransform>().anchorMin;
		cursor.GetComponent<RectTransform>().anchorMax = selectedGameObject.GetComponent<RectTransform>().anchorMax;
		cursor.GetComponent<RectTransform>().pivot = selectedGameObject.GetComponent<RectTransform>().pivot;
		cursor.GetComponent<RectTransform>().rotation = selectedGameObject.GetComponent<RectTransform>().rotation;
		cursor.GetComponent<RectTransform>().localScale = selectedGameObject.GetComponent<RectTransform>().localScale;
	}

	public void OnCancelClick(InputAction.CallbackContext context)
	{
		if (readyParent.activeInHierarchy)
		{
			PlayableCharacter selectedCharacterData = PlayerManager.Instance.players[player.id].character;

			if (selectedCharacterData != null)
			{
				GameObject characterButtonsParent = GameObject.Find("CharacterButtons");

				if (characterButtonsParent != null)
				{
					List<GameObject> characterButtons = new List<GameObject>();
					for (int i = 0; i < characterButtonsParent.transform.childCount; i++)
					{
						Transform child = characterButtonsParent.transform.GetChild(i);
						characterButtons.Add(child.gameObject);
					}

					if (characterButtons != null)
					{
						foreach (GameObject characterButton in characterButtons)
						{
							if (characterButton.GetComponent<PlayableCharacterHolder>().playableCharacter == selectedCharacterData)
							{
								multiplayerEventSystem.SetSelectedGameObject(characterButton);

								// Set canvas variables
								canvasMultiplayerEventSystem.SetSelectedGameObject(GameObject.Find($"MainCanvas/CharacterButtons/" + characterButton.name));
								//canvasCurrentSelectedGameObject = canvasMultiplayerEventSystem.currentSelectedGameObject;
							}
							else
							{
								Debug.Log($"characterButton.playableCharacter is not the same as selectedCharacterData");
							}
						}
					}
					else
					{
						Debug.LogWarning($"characterButtons is null");
					}
				}
				else
				{
					Debug.LogWarning($"characterButtonsParent is null");
				}
			}
			else
			{
				Debug.LogWarning($"selectedCharacterData is null");
			}

			readyParent.SetActive(false);
			
			Image playerCursorImage = playerCursor.GetComponent<Image>();
			playerCursorImage.enabled = true;

			// Set canvas varaibles
			canvasReadyParent.SetActive(false);

			Image canvasCursorImage = canvasCursor.GetComponent<Image>();
			canvasCursorImage.enabled = true;
		}
		else
		{
			Debug.Log($"readyParent is not active in Heirarchy");
		}
	}
}
