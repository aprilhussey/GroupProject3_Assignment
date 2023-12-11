using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player
{
	public int id;
	public int index;
	public PlayerInput input;

	public InputDevice[] devices;
	public string controlScheme;

	public bool isReady;
	public PlayableCharacter character;
	public GameObject characterPrefab;

	public Player(int id, int index, PlayerInput input)
	{
		this.id = id;
		this.index = index;
		this.input = input;

		devices = input.devices.ToArray();
		controlScheme = input.currentControlScheme;

		isReady = false;
		character = null;
		characterPrefab = null;
	}
}
