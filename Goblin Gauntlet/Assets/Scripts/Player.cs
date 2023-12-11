using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
	public int id;
	public int index;
	public PlayerInput input;

	public InputDevice device;
	public string controlScheme;

	public bool isReady;
	public PlayableCharacter character;
	public GameObject characterPrefab;

	public Player(int id, int index, PlayerInput input)
	{
		this.id = id;
		this.index = index;
		this.input = input;

		device = input.devices[0];
		controlScheme = input.currentControlScheme;

		isReady = false;
		character = null;
		characterPrefab = null;
	}
}
