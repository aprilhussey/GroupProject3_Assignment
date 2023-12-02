using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public PlayerInput playerInput;
	public int playerID;

	// Initialize your player here
	public void Initialize(PlayerInput input, int id)
	{
		playerInput = input;
		playerID = id;
	}
}
