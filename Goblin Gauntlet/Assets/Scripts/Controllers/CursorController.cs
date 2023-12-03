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
	private List<GameObject> characterButtons;

	[SerializeField]
	private MultiplayerEventSystem multiplayerEventSystem;

	private GameObject selectedCharacterButton;

	private PlayerInput playerInput;
	private Player player;

	private GameObject playerCharactersParent;
	//private List<GameObject> readyUpButtons;
	private Button[] readyUpButtons;

	// Awake is called before Start
	private void Awake()
	{
		characterButtons = CharacterSelectManager.Instance.characterButtons;
		selectedCharacterButton = characterButtons[0];

		multiplayerEventSystem = this.GetComponent<MultiplayerEventSystem>();
		multiplayerEventSystem.playerRoot = GameObject.Find("Canvas");

		playerInput = this.GetComponent<PlayerInput>();
		player = PlayerManager.Instance.FindPlayerByIndex(playerInput.playerIndex);

		playerCharactersParent = GameObject.Find("PlayerCharacters");
		readyUpButtons = playerCharactersParent.GetComponentsInChildren<Button>(true);
		
	}

	// Start is called before the first frame update
	void Start()
    {
		// Local function to add event listerners
		void AddCharacterButtonListener(GameObject characterButton, Player player)
		{
			characterButton.GetComponent<Button>().onClick.AddListener(() => OnCharacterButtonClick(player));
		}

		void AddReadyUpButtonListener(Button readyUpButton, Player player)
		{
			readyUpButton.onClick.AddListener(() => OnReadyUpButtonClick(player));
		}

		// Subscribe to the OnClick() event of each character button
		foreach (GameObject characterButton in characterButtons)
		{
			AddCharacterButtonListener(characterButton, player);
		}

		// Subscribe to the OnClick() event of each ready up button
		foreach (Button readyUpButton in readyUpButtons)
		{
			AddReadyUpButtonListener(readyUpButton, player);
		}

		multiplayerEventSystem.SetSelectedGameObject(selectedCharacterButton);
	}

    // Update is called once per frame
    void Update()
    {
		if (multiplayerEventSystem.currentSelectedGameObject != null)
		{
			Debug.Log($"Current selected GameObject: {multiplayerEventSystem.currentSelectedGameObject.name}");
			
			MoveCursorToSelectedCharacterButton();
		}
    }

    void MoveCursorToSelectedCharacterButton()
    {
		for (int i = 0; i < characterButtons.Count; i++)
		{
			if (multiplayerEventSystem.currentSelectedGameObject.name == characterButtons[i].name)
			{
				this.gameObject.GetComponent<RectTransform>().anchoredPosition = characterButtons[i].GetComponent<RectTransform>().anchoredPosition;
			}
		}
	}

	public void OnCharacterButtonClick(Player player)
	{
		// Get the currently selected button
		GameObject selectedButton = multiplayerEventSystem.currentSelectedGameObject;

		if (selectedButton != null)
		{
			// Set player.character to selectedButton's playableCharacter
			PlayableCharacterHolder characterHolder = selectedButton.GetComponent<PlayableCharacterHolder>();

			if (characterHolder != null)
			{
				// Set player.character to selectedButton's playableCharacter
				player.character = characterHolder.playableCharacter;
				CharacterSelectManager.Instance.OnCharacterButtonClicked(ref player);
			}
			else
			{
				Debug.LogError("PlayableCharacterHolder component not found on the selected button.");
			}
		}
		else
		{
			Debug.LogError("Selected button is null.");
		}
	}

	public void OnReadyUpButtonClick(Player player)
	{
		CharacterSelectManager.Instance.OnReadyUpButtonClicked(ref player);
	}

	private void OnDestroy()
	{
		foreach (GameObject characterButton in characterButtons)
		{
			characterButton.GetComponent<Button>().onClick.RemoveListener(() => OnCharacterButtonClick(player));
		}

		foreach (Button readyUpButton in readyUpButtons)
		{
			readyUpButton.onClick.RemoveListener(() => OnReadyUpButtonClick(player));
		}
	}
}
