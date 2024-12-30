using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;

public class CommonVariables : MonoBehaviour // a class for public static variables which need to remain consistant while a game's active and there can't be multiple instances of them
{
    // ----------------- GAME STATE BOOLEAN VALUES ---------------

    public static bool GameActive = true; // Boolean property to signify if a game's active or not. Used by some methods for validation and to improve security
    public static bool PlayerTurn = true;  // Boolean property to signify it's currently the players turn (true by default as player starts by default)
    public static bool Paused = false; // Boolean property to signify if the game is paused or not

    // --------------------- TIME PROPERTIES ---------------------

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
                else
                {
                    Debug.LogError($"Invalid value inputted for PlayerTimeLeft. Value given == {value}. No changes applied"); // If validation fails an errors outputted for testing
                }
            }
            else
            {
                playerTimeLeft = value;
            }
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
            else
            {
                Debug.LogError($"Invalid value inputted for AITimeLeft. Value given == {value}. No changes applied"); // If validation fails an errors outputted for testing
            }
        }
    }

    // --------------------- SCORE PROPERTIES ---------------------

    // Player score property (encapsulated + validated) 
    private static int playerScore; 
    static public int PlayerScore
    {
        get { return playerScore; }
        set 
        { 
            if (value == playerScore + 1 || value == 0) // Validates new value's either incremented or reset
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
}
