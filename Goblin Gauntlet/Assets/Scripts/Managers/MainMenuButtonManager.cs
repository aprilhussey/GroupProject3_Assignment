using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
	// Singleton instance
	//public static MainMenuButtonManager Instance = null;

	[Header("Main Menu Buttons")]
	public GameObject mainMenuCanvas;
	public GameObject mainMenuFirstButton;

	private SettingsButtonManager settingsButtonManager = null;

	// Awake is called before Start
	void Awake()
	{
		// Ensure only one instance exists
		/*if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}*/
	}

	void Start()
	{
		settingsButtonManager = GameObject.Find("SettingsButtonManager").GetComponent<SettingsButtonManager>();
	}

	public void OnPlayButtonClick()
	{
		GameManager.Instance.ChangeGameState(GameState.CharacterSelect);
	}

	public void OnSettingsButtonClick()
	{
		Scene currentScene = SceneManager.GetActiveScene();

		if (currentScene.name == "MainMenu")
		{
			mainMenuCanvas.SetActive(false);
			//SettingsButtonManager.Instance.settingsCanvas.SetActive(true);
			settingsButtonManager.settingsCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			//EventSystem.current.SetSelectedGameObject(SettingsButtonManager.Instance.settingsFirstButton);
			EventSystem.current.SetSelectedGameObject(settingsButtonManager.settingsFirstButton);
		}

		/*if (GameManager.Instance.gameState == GameState.MainMenu)
		{
			mainMenuCanvas.SetActive(false);
			//SettingsButtonManager.Instance.settingsCanvas.SetActive(true);
			settingsButtonManager.settingsCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			//EventSystem.current.SetSelectedGameObject(SettingsButtonManager.Instance.settingsFirstButton);
			EventSystem.current.SetSelectedGameObject(settingsButtonManager.settingsFirstButton);
		}*/
	}

	public void OnQuitButtonClick()
	{
		Application.Quit();
	}

	public void OnCreditsButtonClick()
	{
		GameManager.Instance.ChangeGameState(GameState.Credits);
	}

	public void OnBackClick()
	{
		/*if (SettingsButtonManager.Instance.settingsCanvas.activeInHierarchy)
		{
			SettingsButtonManager.Instance.settingsCanvas.SetActive(false);
			mainMenuCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			EventSystem.current.SetSelectedGameObject(SettingsButtonManager.Instance.settingsClosedButton);
		}*/
		if (settingsButtonManager.settingsCanvas.activeInHierarchy)
		{
			settingsButtonManager.settingsCanvas.SetActive(false);
			mainMenuCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			EventSystem.current.SetSelectedGameObject(settingsButtonManager.settingsClosedButton);
		}
	}
}
