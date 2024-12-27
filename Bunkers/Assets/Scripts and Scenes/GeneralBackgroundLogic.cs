using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralBackgroundLogic : MonoBehaviour
{
    [SerializeField] GameObject instanceReferences;
    public static void StartGame() // Procedure to start a new game
    {
        Debug.Log("Attempting to start a singleplayer game"); // Outputs a log to the debug console for testing
        ResetGame(false); // Calls ResetGame function (with false to signify not full reset) to ensure game is reset (should already be)
        CommonVariables.Paused = false; // Makes sure the game's unpaused
        CommonVariables.GameActive = true; // Sets gameactive == true for other methods and functions to work properly (eg timers)
        SceneManager.LoadScene("Gamescene"); // Loads the gamescene
    }

    public static void ChangeTurn() // Procedure to switch turns between AI and the player
    {
        CommonVariables.PlayerTurn = !CommonVariables.PlayerTurn; // Changes turn to whatever the current turn isn't
        Debug.Log($"Turn changed to {CommonVariables.PlayerTurn}"); // Outputs a message to the debug console for testing and debugging
    }

    public static void EndGame(int winner) // Procedure to end a game due to someone winning through game logic
    {
        CommonVariables.GameActive = false; // makes the gameactive variable false
        Time.timeScale = 1f;

        if (winner == 0) // Checks if winner = 0. 0 signifies that the player won
        {
            CommonVariables.PlayerScore += 1; // increments player score by 1 (as they won)

        }
        else if (winner == 1)
        {
            CommonVariables.AIScore += 1;

            AdditiveGameMenus.Instance.EnableEndScreen(); // Enables end screen overlay after scores have been updated so the end screen updates with the new score values

            // Will update statistics in later iteration
        }
    } // End of EndGame method

    public static void ForceEndGame() // Procedure to end a game due to the player exiting either through the pause menu or end game screen. 
    {
        // maybe update statistics for games quit
        ResetGame(true); // Calls the reset game function and passes in true to signify it's a full reset
    }

    public static bool Rematch() // function to play another game (when one's lost)
    {
        ResetGame(false); // Calls ResetGame function to reset game state to default. Inputs false to not full reset so scores remain
        SceneManager.LoadScene("Gamescene"); // Loads the gamescene again
        return true; // returns false to say method was unsucessful at completing (important as menu's mustn't close if this fails).
    }

    public static void ResetGame(bool fullReset)
    {
        InstanceReferences.instance.TimerScriptInstance.ResetTime(); // Calls the ResetTime procedure to set times back to the default

        if (fullReset) // If full reset is initated it means another game isn't about to be played immediately after. Resets scores
        {
            CommonVariables.AIScore = 0;
            CommonVariables.PlayerScore = 0;
        }
    }

} // ------------------ End of GeneralBackgroundGameLogic class ------------------
