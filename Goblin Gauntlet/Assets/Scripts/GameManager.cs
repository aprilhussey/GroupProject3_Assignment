using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
		if (scene.name == "Game")
		{
			playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			artifactController = GameObject.FindWithTag("Artifact").GetComponent<ArtifactController>();

			gameOver = GameObject.Find("GameOver");
			gameOver.SetActive(false);

			paused = GameObject.Find("Paused");
			paused.SetActive(false);
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
