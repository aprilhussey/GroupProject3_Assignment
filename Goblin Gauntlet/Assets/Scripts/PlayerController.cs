using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayableCharacter characterData;
    
    // Start is called before the first frame update
    void Start()
    {
        // Access character data
        string id = characterData.id;
        CharacterClass characterClass = characterData.characterClass;
        string characterName = characterData.characterName;
        int health = characterData.health;
        Ability mainAbility = characterData.mainAbility;
        Ability specialAbility = characterData.specialAbility;

        // TODO: Use character data to initialize player
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Handle player input and update player state
    }
}
