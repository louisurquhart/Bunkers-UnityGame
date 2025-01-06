using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;

public class CommonVariables : MonoBehaviour // a class for public static variables which need to remain consistant while a game's active and there can't be multiple instances of them
{
    // ------------------------------ GAME STATE BOOLEAN VALUES -------------------------------------

    public static bool GameActive = false; // Boolean property to signify if a game's active or not. Used by some methods for validation and to improve security
    public static bool PlayerTurn = true;  // Boolean property to signify it's currently the players turn (true by default as player starts by default)
    public static bool Paused = false; // Boolean property to signify if the game is paused or not


    // ----------------- INTEGER PROPERTIES FOR CALCULATION (ENCAPSULATED + VALIDATED) ---------------

    // --- TIME PROPERTIES ---

    // Player timer property
    private static int playerTimeLeft;
    public static int PlayerTimeLeft // Int variable to hold the amount of time left for the player
    {
        // Encapsulated using getters + setters to apply range validation
        get { return playerTimeLeft; }
        set
        {
            if (GameActive)
            {
                if (value >= 0 && value <= 900) // Validates given time value's within bounds 0-900 
                {
                    playerTimeLeft = value;
                }
                else { Debug.LogError($"Invalid value inputted for PlayerTimeLeft. Value given == {value}. No changes applied"); }// If validation fails an errors outputted for testing}
            }
            else { playerTimeLeft = value; }
        }
    }

    // AI timer property
    private static int aiTimeLeft;
    public static int AITimeLeft // Int variable to hold the amount of time left for the AI
    {
        // Encapsulated using getters + setters to apply range validation
        get { return aiTimeLeft; }
        set
        {
            if (value >= 0 && value <= 900) // Validates given time value's within bounds 0-900 
            {
                aiTimeLeft = value;
            }
            else { Debug.LogError($"Invalid value inputted for AITimeLeft. Value given == {value}. No changes applied"); } // If validation fails an errors outputted for testing}
        }
    }

    // --- SCORE PROPERTIES ---

    // Player score property (encapsulated + validated) 
    private static int playerScore;
    static public int PlayerScore
    {
        get { return playerScore; }
        set
        {
            if (value == playerScore + 1 || value == 0) // Validates that the given value's either incrementing or reseting the score + range validation
            {
                playerScore = value; // If so the encapsulated value's set to the input
            }
            else
            {
                Debug.LogError($"Invalid value inputted for PlayerScore. Value given == {value}, Previous Value == {playerScore}. No changes applied"); // If validation fails an errors outputted for testing
            }
        }
    }

    // AI score property (encapsulated + validated)

    private static int aiScore = 0;
    static public int AIScore
    {
        get { return aiScore; }
        set
        {
            if (value == aiScore + 1 || value == 0)  // Validates new value's either incremented or reset
            {
                aiScore = value; // If so the encapsulated value's set to the input
            }
            else
            {
                Debug.LogError($"Invalid value inputted for AIScore. Value given == {value}, Previous Value == {playerScore}. No changes applied"); // If validation fails an errors outputted for testing
            }
        }
    }


    // --- BUNKER COUNT PROPERTIES + DICTIONARY ---

    // Player alive bunker count property (encapsulated in case of future validation requirements) 
    static private int playerAliveFullBunkerCount;
    static public int PlayerAliveFullBunkerCount
    {
        get { return playerAliveFullBunkerCount; }
        set { playerAliveFullBunkerCount = value; } 
    }

    // AI alive bunker count property (encapsulated in case of future validation requirements) 
    static private int aiAliveFullBunkerCount;
    static public int AIAliveFullBunkerCount
    {
        get { return aiAliveFullBunkerCount; }
        set { aiAliveFullBunkerCount = value; }
    }


    // Dictionary to return corrosponding bunker count property reference depending on a int entity reference (0 = PLAYER, 1 = AI)
    public static readonly Dictionary<int, (Func<int> Get, Action<int> Set)> BunkerCountsDictionary = new Dictionary<int, (Func<int> Get, Action<int> Set)>
    {
        { 0, (() => PlayerAliveFullBunkerCount, value => PlayerAliveFullBunkerCount = value) },
        { 1, (() => AIAliveFullBunkerCount, value => AIAliveFullBunkerCount = value) }
    };


    // ------------------------------ STRING FORMATTING VARIABLES -------------------------------------

    // -- DEBUG MESSAGE FORMAT

    public static string[] DebugFormat = new string[2]
    {
        "<color=green>", // Player (Green)
        "<color=#fd3a3a>" // AI (Light red)
    };
}

