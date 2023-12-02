using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CursorController : MonoBehaviour
{
	private List<GameObject> characterButtons;

	[SerializeField]
	private MultiplayerEventSystem multiplayerEventSystem;

	private GameObject selectedCharacterButton;

	// Awake is called before Start
	private void Awake()
	{
		characterButtons = CharacterSelectManager.instance.characterButtons;
		selectedCharacterButton = characterButtons[0];

		multiplayerEventSystem = this.GetComponent<MultiplayerEventSystem>();
		multiplayerEventSystem.playerRoot = GameObject.Find("Canvas");
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
}
