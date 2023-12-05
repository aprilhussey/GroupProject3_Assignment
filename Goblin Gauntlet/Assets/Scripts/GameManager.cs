using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	// Singleton instance
	public static GameManager Instance = null;

	// Game state
	public enum GameState
	{
		MainMenu,
		CharacterSelection,
		Game,
		Paused
	}

	public GameState gameState;

	public LayerMask playerLayer;
	public LayerMask enemyLayer;
	public LayerMask obstructionLayer;

	[SerializeField]
	private GameObject PaladinAzrealPrefab;
	[SerializeField]
	private GameObject WarlockDahliaPrefab;
	[SerializeField]
	private GameObject ClericEvePrefab;
	[SerializeField]
	private GameObject RogueZezioPrefab;

	[SerializeField]
	private Vector3 spawnLocation;

	// TEMP //
	// TEMP //
	public PlayerController playerController;
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
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game" || scene.name == "Level001" || scene.name == "Level002")
		{
			//playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			artifactController = GameObject.FindWithTag("Artifact").GetComponent<ArtifactController>();

			gameOver = GameObject.Find("GameOver");
			gameOver.SetActive(false);

			paused = GameObject.Find("Paused");
			paused.SetActive(false);

			foreach (Player player in PlayerManager.Instance.players)
			{
				// Instantiate the correct prefab based on the player's index or selection
				/*GameObject playerPrefab = Instantiate(GetPlayerPrefab(player.index), spawnLocation, Quaternion.identity);
				PlayerInput playerInputComponent = playerPrefab.GetComponent<PlayerInput>();

				// Transfer the PlayerInput component from the old character to the new one
				InputSystem.DisableDevice(player.input.devices[0]); // Disable the old input device
				playerInputComponent.SwitchCurrentControlScheme(player.input.currentControlScheme, player.input.devices); // Switch control scheme
				InputSystem.EnableDevice(player.input.devices[0]); // Re-enable the input device*/

				player.input.SwitchCurrentActionMap("Player");
			}
		}
	}

	private GameObject GetPlayerPrefab(int index)
	{
		// Return the correct prefab based on the player's index or selection
		// This is just an example, you'll need to implement your own logic here
		switch (index)
		{
			case 0: return PaladinAzrealPrefab;
			case 1: return WarlockDahliaPrefab;
			case 2: return ClericEvePrefab;
			case 3: return RogueZezioPrefab;
			default: return null;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (playerController != null && artifactController != null)
		{
			if (playerController.health <= 0 || artifactController.health <= 0)
			{
				ChangeGameState(GameState.Game);
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

			case GameState.CharacterSelection:
				SceneManager.LoadScene("CharacterSelection");
				break;
			
			case GameState.Game:
				SceneManager.LoadScene("Game");
				break;

			case GameState.Paused:
				// Handle pause logic here
				break;
		}
	}

	// TEMP //
	// TEMP //
	public void StartButton()
	{
		ChangeGameState(GameState.Game);
	}

	public void OkayButton()
	{
		ChangeGameState(GameState.MainMenu);
	}

	public void ResumeButton()
	{
		paused.SetActive(false);
		Time.timeScale = 1f;
		isGamePaused = false;
	}

	public void MainMenuButton()
	{
		ChangeGameState(GameState.MainMenu);
	}

	// TEMP //
	// TEMP //
}
