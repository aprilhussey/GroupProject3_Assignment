using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
	// Singleton instance
	public static GameManager Instance = null;

	// Game state
	public enum GameState
	{
		MainMenu,
		CharacterSelect,
		Game,
		Paused,
		Level001,
		Level002,
		Credits
	}

	public GameState gameState;

	public LayerMask playerLayer;
	public LayerMask enemyLayer;
	public LayerMask obstructionLayer;

	[SerializeField]
	private Vector3 spawnLocation;

	// TEMP //
	// TEMP //
	//public PlayerController playerController;
	public ArtifactController artifactController;

	public GameObject gameOver;
	public GameObject paused;

	private InputActions inputActions;

	public Button okayButton;
	public Button resumeButton;

	public static bool isGamePaused = false;
	// TEMP //
	// TEMP //


	[Header("Main Menu Buttons")]
	[SerializeField]
	private GameObject mainMenuCanvas;
	[SerializeField]
	private GameObject mainMenuFirstButton;

	[Header("Settings Buttons")]
	[SerializeField]
	private GameObject settingsCanvas;
	[SerializeField]
	private GameObject settingsFirstButton;
	[SerializeField]
	private GameObject settingsClosedButton;


	public GameObject volumeButton;
	[SerializeField]
	private GameObject volumeSlider;

	[Header("Credits Buttons")]
	[SerializeField]
	private GameObject creditsFirstButton;
	[SerializeField]
	private GameObject creditsClosedButton;

	// Awake is called before Start
	void Awake()
	{
		// Ensure only one GameManager instance exists
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy GameManager when switching scenes
		DontDestroyOnLoad(gameObject);

		// Initialize game state
		gameState = GameState.MainMenu;

		// Set layer mask variables to their respective layers
		playerLayer = LayerMask.GetMask("Player");
		enemyLayer = LayerMask.GetMask("Enemy");
		obstructionLayer = LayerMask.GetMask("Obstruction");
	}

	// TEMP //
	// TEMP //
	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;

		inputActions = new InputActions();
		inputActions.Enable();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game" || scene.name == "Level001" || scene.name == "Level002")
		{
			//playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			artifactController = GameObject.FindWithTag("Artifact").GetComponent<ArtifactController>();

			//gameOver = GameObject.Find("GameOver");
			//gameOver.SetActive(false);

			//paused = GameObject.Find("Paused");
			//paused.SetActive(false);

			/*foreach (Player player in PlayerManager.Instance.players)
			{
				// Instantiate the correct prefab based on the player's index or selection
				GameObject playerPrefab = Instantiate(GetPlayerPrefab(player.index), spawnLocation, Quaternion.identity);
				PlayerInput playerInputComponent = playerPrefab.GetComponent<PlayerInput>();

				// Transfer the PlayerInput component from the old character to the new one
				InputSystem.DisableDevice(player.input.devices[0]); // Disable the old input device
				playerInputComponent.SwitchCurrentControlScheme(player.input.currentControlScheme, player.input.devices); // Switch control scheme
				InputSystem.EnableDevice(player.input.devices[0]); // Re-enable the input device

				//player.input.SwitchCurrentActionMap("Player");
			}*/
		}

		if (scene.name == "MainMenu")
		{
			mainMenuCanvas.SetActive(true);
			settingsCanvas.SetActive(false);

			EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (artifactController != null)
		{
			if (artifactController.currentHealth <= 0)
			{
				ChangeGameState(GameState.MainMenu);
				/*Time.timeScale = 0f;
				gameOver.SetActive(true);
				okayButton = GameObject.Find("Okay").GetComponent<Button>();
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(okayButton.gameObject);

				isGamePaused = true;*/
			}

			if (inputActions.Player.Pause.triggered)
			{
				if (!paused.activeInHierarchy)
				{
					Time.timeScale = 0f;
					paused.SetActive(true);
					resumeButton = GameObject.Find("Resume").GetComponent<Button>();
					EventSystem.current.SetSelectedGameObject(null);
					EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

					isGamePaused = true;
				}
				else if (paused.activeInHierarchy)
				{
					Time.timeScale = 1f;
					paused.SetActive(false);
					EventSystem.current.SetSelectedGameObject(null);

					isGamePaused = false;
				}
			}
		}
	}
	// TEMP //
	// TEMP //

	public void ChangeGameState(GameState newState)
	{
		gameState = newState;

		switch (gameState)
		{
			case GameState.MainMenu:
				SceneManager.LoadScene("MainMenu");
				break;

			case GameState.CharacterSelect:
				SceneManager.LoadScene("CharacterSelect");
				break;
			
			case GameState.Game:
				SceneManager.LoadScene("Game");
				break;

			case GameState.Level001:
				SceneManager.LoadScene("Level001");
				break;

			case GameState.Level002:
				SceneManager.LoadScene("Level002");
				break;

			case GameState.Paused:
				// Handle pause logic here
				break;

			case GameState.Credits:
				SceneManager.LoadScene("Credits");
				break;
		}
	}

	// MAIN MENU BUTTONS //
	public void OnPlayButtonClick()
	{
		ChangeGameState(GameState.CharacterSelect);
	}

	public void OnSettingsButtonClick()
	{
		if (gameState == GameState.MainMenu)
		{
			mainMenuCanvas.SetActive(false);
			settingsCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			EventSystem.current.SetSelectedGameObject(settingsFirstButton);
		}
	}

	public void OnQuitButtonClick()
	{
		Application.Quit();
	}

	public void OnCreditsButtonClick()
	{
		ChangeGameState(GameState.Credits);
	}

	// WIN / LOSE BUTTONS //
	public void OnOkayButtonClick()
	{
		ChangeGameState(GameState.MainMenu);
	}

	public void OnResumeButtonClick()
	{
		paused.SetActive(false);
		Time.timeScale = 1f;
		isGamePaused = false;
	}

	public void OnMainMenuButtonClick()
	{
		ChangeGameState(GameState.MainMenu);
	}

	// SETTINGS BUTTONS //
	public void OnSoundButtonClick()
	{
		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		EventSystem.current.SetSelectedGameObject(volumeSlider);
	}

	public void OnBackClick()
	{
		if (settingsCanvas.activeInHierarchy)
		{
			settingsCanvas.SetActive(false);
			mainMenuCanvas.SetActive(true);

			// Clear selected button
			EventSystem.current.SetSelectedGameObject(null);
			// Set selected button
			EventSystem.current.SetSelectedGameObject(settingsClosedButton);
		}
	}
}
