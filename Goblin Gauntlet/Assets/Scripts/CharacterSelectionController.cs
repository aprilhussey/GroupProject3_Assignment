using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    private int playerIndex;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject menuPanel;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex (int inputPlayerIndex)
    {
        playerIndex = inputPlayerIndex;
        titleText.SetText("Player " + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
    {
        if (!inputEnabled)
        {
            return;
        }

        PlayerConfigurationManager.instance.SetPlayerColor(playerIndex, color);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
		if (!inputEnabled)
		{
			return;
		}

        PlayerConfigurationManager.instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
	}
}
