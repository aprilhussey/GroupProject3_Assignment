using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

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

	List<GameObject> playerGameObjects;
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
		Time.timeScale = 1.0f;

		if (scene.name == "Game" || scene.name == "Level001" || scene.name == "Level002")
		{
			playerGameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
			artifactController = GameObject.FindWithTag("Artifact").GetComponent<ArtifactController>();

			gameOver = GameObject.FindWithTag("GameOver");
			gameOver.SetActive(false);

			//paused = GameObject.Find("Paused");
			//paused.SetActive(false);
		}

		if (scene.name == "MainMenu")
		{
			if (mainMenuButtonManager != null || settingsButtonManager != null)
			{
				mainMenuButtonManager.mainMenuCanvas.SetActive(true);
				settingsButtonManager.settingsCanvas.SetActive(false);

				EventSystem.current.SetSelectedGameObject(mainMenuButtonManager.mainMenuFirstButton);
			}
		}

		if (scene.name == "CharacterSelect")
		{
			PlayerManager.Instance.players.Clear();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (artifactController != null && gameOver != null)
		{
			if (artifactController.currentHealth <= 0)
			{
				gameOver.SetActive(true);
				artifactController = null;
				EventSystem.current.SetSelectedGameObject(gameOver.GetComponentInChildren<Button>().gameObject);
			}
		}

		if (playerGameObjects != null && gameOver != null)
		{
			int numberOfPlayers = playerGameObjects.Count;
			int deadPlayers = 0;

			for (int i = 0; i < numberOfPlayers; i++)
			{
				PlayerController playerController = playerGameObjects[i].GetComponent<PlayerController>();
				if (playerController.currentHealth <= 0)
				{
					//Destroy(playerController.gameObject);
					deadPlayers++;
				}
			}

			if (deadPlayers == numberOfPlayers)
			{
				Time.timeScale = 0f;
				playerGameObjects.Clear();
				gameOver.SetActive(true);
				EventSystem.current.SetSelectedGameObject(gameOver.GetComponentInChildren<Button>().gameObject);
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
