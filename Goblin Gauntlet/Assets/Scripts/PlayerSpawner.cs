using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject paladinAzrealPrefab;
	[SerializeField]
	private GameObject warlockDahliaPrefab;
	[SerializeField]
	private GameObject clericEvePrefab;
	[SerializeField]
	private GameObject rogueZezioPrefab;
	
	[SerializeField]
	private GameObject spawnLocation;

	void Start()
	{
		foreach (Player player in PlayerManager.Instance.players)
		{
			// Instantiate the character prefab
			GameObject playerInstance = Instantiate(GetPrefabFromCharacter(player.character), spawnLocation.transform.position, Quaternion.identity);

			// Transfer the PlayerInput component
			PlayerInput existingPlayerInput = playerInstance.GetComponent<PlayerInput>();
			if (existingPlayerInput != null)
			{
				Destroy(existingPlayerInput); // Remove the new PlayerInput component
			}
			player.input.transform.SetParent(playerInstance.transform); // Re-parent the original PlayerInput to the new instance
			player.input.SwitchCurrentControlScheme(player.input.currentControlScheme); // Refresh the control scheme if needed
		}
	}

	private GameObject GetPrefabFromCharacter(PlayableCharacter character)
	{
		Debug.Log($"character: {character}");

		if (character.name == "PaladinAzreal")
		{
			return paladinAzrealPrefab;
		}
		else if (character.name == "WarlockDahlia")
		{
			return warlockDahliaPrefab;
		}
		else if (character.name == "ClericEve")
		{
			return clericEvePrefab;
		}
		else if (character.name == "RogueZezio")
		{
			return rogueZezioPrefab;
		}
		else
		{
			return null;
		}
	}
}
