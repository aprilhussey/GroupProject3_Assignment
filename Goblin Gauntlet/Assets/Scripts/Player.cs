using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
	public int id;
	public int index;
	public PlayerInput input;
	public bool isReady;
	public PlayableCharacter character;
	public GameObject characterPrefab;

	public Player(int id, int index, PlayerInput input)
	{
		this.id = id;
		this.input = input;
		this.index = index;
		isReady = false;
		character = null;
		characterPrefab = null;
	}
}
