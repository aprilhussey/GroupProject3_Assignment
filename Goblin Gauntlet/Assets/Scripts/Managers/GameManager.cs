using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Singleton instance
	public static GameManager Instance = null;

	private SettingsButtonManager settingsButtonManager = null;
	private MainMenuButtonManager mainMenuButtonManager = null;

	// Game state
	public enum GameState
	{
		MainMenu,
		CharacterSelect,
		Game,
		Paused,
		Level001,
		Level002,
		Credits,
		LoadingScene001,
		LoadingScene002
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

		settingsButtonManager = GameObject.Find("SettingsButtonManager").GetComponent<SettingsButtonManager>();
		mainMenuButtonManager = GameObject.Find("MainMenuButtonManager").GetComponent<MainMenuButtonManager>();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game" || scene.name == "Level001" || scene.name == "Level002")
		{
			//playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			//artifactController = GameObject.FindWithTag("Artifact").GetComponent<ArtifactController>();

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
			/*MainMenuButtonManager.Instance.mainMenuCanvas.SetActive(true);
			SettingsButtonManager.Instance.settingsCanvas.SetActive(false);

			EventSystem.current.SetSelectedGameObject(MainMenuButtonManager.Instance.mainMenuFirstButton);
			*/
			if (mainMenuButtonManager != null || settingsButtonManager != null)
			{
				mainMenuButtonManager.mainMenuCanvas.SetActive(true);
				settingsButtonManager.settingsCanvas.SetActive(false);

				EventSystem.current.SetSelectedGameObject(mainMenuButtonManager.mainMenuFirstButton);
			}
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

            case GameState.LoadingScene001:
                SceneManager.LoadScene("LoadingScene001");
                break;

            case GameState.LoadingScene002:
                SceneManager.LoadScene("LoadingScene002");
                break;

            case GameState.Paused:
				// Handle pause logic here
				break;

			case GameState.Credits:
				SceneManager.LoadScene("Credits");
				break;
		}
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
}
