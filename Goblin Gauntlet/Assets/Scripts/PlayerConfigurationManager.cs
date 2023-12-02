using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField] private int maxPlayers = 2;

    public static PlayerConfigurationManager instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("WARNING: Another instance of PlayerConfigurationManager SINGLETON is trying to be made");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].playerMaterial = color;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;

        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            Debug.Log($"All players are ready load game");
        }
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        Debug.Log($"Player joined = {playerInput.playerIndex}");

        if (!playerConfigs.Any(p => p.playerIndex == playerInput.playerIndex))
        {
			playerInput.transform.SetParent(transform);
			playerConfigs.Add(new PlayerConfiguration(playerInput));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        playerIndex = playerInput.playerIndex;
        input = playerInput;
    }

    public PlayerInput input { get; set; }
    public int playerIndex { get; set; }
    public bool isReady { get; set; }
    public Material playerMaterial { get; set; }
}
