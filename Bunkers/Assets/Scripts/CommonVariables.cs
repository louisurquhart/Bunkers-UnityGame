using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommonVariables : MonoBehaviour // a class for public static variables which need to remain consistant while a game's active and there can't be multiple instances of them
{
    // Game/Turn state properties
    static public bool GameActive = false; // Boolean property to signify if a game's active or not. Used by some methods for validation and to improve security
    static public bool PlayerTurn = true;  // Boolean property to signify it's currently the players turn (true by default as player starts by default)
    public static bool Paused = false; // Boolean property to signify if the game is paused or not

    // Bunker count properties. Validated to prevent an issue with the code from setting them to impossible values

    static private int _playerBunkerCount = 16; // Private bunkercount variable which needs to be accessed through getters + setters
    static public int PlayerBunkerCount // Public bunkercount which has getters + setters to validate results
    { 
        get { return _playerBunkerCount; } 
        set { if (PlayerBunkerCount >= 0 & PlayerBunkerCount < 16) { _playerBunkerCount = PlayerBunkerCount; } } 
    }

    static private int _aiBunkerCount = 16;
    static public int AIBunkerCount 
    { 
        get { return _aiBunkerCount; } 
        set { if (AIBunkerCount >= 0 & AIBunkerCount < 16) { _aiBunkerCount = AIBunkerCount; } } 
    }

    // Score properties
    static public int PlayerScore = 0;
    static public int AIScore = 0;
    
}
