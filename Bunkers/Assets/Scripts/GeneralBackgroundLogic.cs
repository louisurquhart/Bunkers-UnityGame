using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralBackgroundLogic : MonoBehaviour
{
    public static void StartGame()
    {
        Debug.Log("Attempting to start a singleplayer game"); // Outputs a log to the debug console for testing
        SceneManager.LoadScene("Gamescene"); // Loads the gamescene
        CommonVariables.Paused = false; // Makes sure the game's unpaused
        CommonVariables.GameActive = true; // Sets gameactive == true for other methods and functions to work properly (eg timers)
    }

    public static void ChangeTurn() // procedure to switch turns between AI and the player
    {

        if (CommonVariables.PlayerTurn) // If it's currently the player turn
        {
            CommonVariables.PlayerTurn = false; //  PlayerTurn is set to false making it the AI's turn
            Debug.Log("Turn changed from AI's turn -> Players turn"); // outputs message to debug log that the turns changed (how its changed too) for debugging + testing
                                                                      // Will need to call AITurn procedure when it's made
        }
        else // Otherwise if it's not the players turn it assumes it's the AI's turn
        {
            CommonVariables.PlayerTurn = true; // PlayerTurn is set to true making it the players turn.
            Debug.Log("Turn changed from Players turn -> AI's turn"); // outputs message to debug log that the turns changed (how its changed too) for debugging + testing
            // Will need to call PlayerTurn procedure when it's made
        }
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

            AdditiveGameMenus.Instance.EnableEndScreen(); // enables end screen overlay after scores have been updated so they're updated on the end screen

            // need to update statistics
        }
    } // End of EndGame method

    public static void ForceEndGame() // Procedure to end a game due to the player exiting either through the pause menu or end game screen. 
    {
        // maybe update statistics for games quit
        ResetGame(true); // Calls the reset game function and passes in true to signify it's a full reset
    }

    public static bool Rematch() // function to play another game (when one's lost)
    {

        return true; // returns false to say method was unsucessful at completing (important as menu's mustn't close if this fails).
    }

    public static void ResetGame(bool fullReset)
    {
        TimerScripts.Instance.ResetTime(); // Calls the ResetTime procedure to set times back to the default


        if (fullReset) // If full reset is initated it means another game isn't about to be played immediately after
        {
            CommonVariables.AIScore = 0;
            CommonVariables.PlayerScore = 0;
        }
    }

} // ------------------ End of GeneralBackgroundGameLogic class ------------------
