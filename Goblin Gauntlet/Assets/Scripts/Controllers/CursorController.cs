using System.Collections;
using System.Collections.Generic;
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

	// Awake is called before Start
	private void Awake()
	{
		characterButtons = CharacterSelectManager.Instance.characterButtons;
		selectedCharacterButton = characterButtons[0];

		multiplayerEventSystem = this.GetComponent<MultiplayerEventSystem>();
		multiplayerEventSystem.playerRoot = GameObject.Find("Canvas");

		playerInput = this.GetComponent<PlayerInput>();

		// Subscribe to the OnClick() event of each button
		foreach (GameObject button in characterButtons)
		{
			button.GetComponent<Button>().onClick.AddListener(() => OnCharacterButtonClick(playerInput));
		}
	}

	// Start is called before the first frame update
	void Start()
    {
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

	public void OnCharacterButtonClick(PlayerInput playerInput)
	{
		// Access the player who clicked the button
		Player player = PlayerManager.Instance.FindPlayerByIndex(playerInput.playerIndex);

		// Get the currently selected button
		GameObject selectedButton = multiplayerEventSystem.currentSelectedGameObject;

		// Get PlaybaleCharacterHolder component from the selected button
		PlayableCharacter playableCharacter = selectedButton.GetComponent<PlayableCharacterHolder>().playableCharacter;

		CharacterSelectManager.Instance.OnCharacterButtonClicked(player, playableCharacter);
	}

	private void OnDestroy()
	{
		foreach (GameObject button in characterButtons)
		{
			button.GetComponent<Button>().onClick.RemoveListener(() => OnCharacterButtonClick(playerInput));
		}
	}
}
