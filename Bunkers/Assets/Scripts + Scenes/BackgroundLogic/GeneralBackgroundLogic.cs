using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralBackgroundLogic : MonoBehaviour
{
    // ------------- START GAME FUNCTIONALITY -----------------
    public static void StartGame()
    {
        Debug.Log("Attempting to start a singleplayer game"); // Outputs a debug log for testing purposes 
        CommonVariables.GameActive = true; // Sets gameactive = true 
        SceneManager.LoadScene("Gamescene"); // Loads the gamescene
        StatisticsMenuFunctionality.IncrementStatisticValue("TotalGamesLaunched");
    }


    // -------------------- CHANGE TURN ---------------------
    public static void ChangeTurn() // Procedure to switch turns between AI and the player
    {
        CommonVariables.PlayerTurn = !CommonVariables.PlayerTurn; // Changes turn to whatever the current turn isn't

        if (CommonVariables.PlayerTurn) // If the new turn's the players turn
        {
            Debug.Log($"<b><color=white>PLAYER TURN:"); // Debug log for identifying which turn changes happened in testing
            // Needs to prompt player it's their turn
        }
        else // If the new turn value's the AI turn
        {
            Debug.Log($"<b><color=white>AI TURN:"); // Debug log for identifying which turn changes happened in testing
            AILogic.InitiateAITurn(); // If turn's changed to AI it calls the AI's turn
        }
    }


    // --------------------- END GAME FUNCTIONALITY + WIN/LOSS CHECKS ------------------------

    // Procedure to check if a game has ended and the entity type inputted (0 = player, 1 = AI)
    public static void HasGameEnded() 
    {
        if (CommonVariables.PlayerAliveFullBunkerCount <= 0) // If the player has no more full bunkers remaining they've lost
        {
            Debug.Log($"<b>{CommonVariables.DebugFormat[1]}HasGameEnded: Determined AI has won (PlayerAliveBunkerCount: {CommonVariables.PlayerAliveFullBunkerCount} (should be 0))"); // Debug log for testing
            EndGame(1); // End game function. 1 is passed in to signify the AI won
        }
        else if (CommonVariables.AIAliveFullBunkerCount <= 0) // If the AI has no more full bunkers remaining they've lost
        {
            Debug.Log($"<b>{CommonVariables.DebugFormat[0]}HasGameEnded: Determined player has won (AIAliveBunkerCount: {CommonVariables.AIAliveFullBunkerCount} (should be 0)"); // Debug log for testing
            EndGame(0); // End game function. 0 is passed in to signify the player won
        }
    }

    // Procedure to end a game due to someone winning through game logic (0 = player, 1 = AI)
    public static void EndGame(int winner) 
    {
        ResetGame(false); 

        // Statistics update
        if (winner == 0) // If player won
        {
            CommonVariables.PlayerScore++; // Incrments the players score
            StatisticsMenuFunctionality.IncrementStatisticValue("Wins"); // Increments the players win statistic
        }
        else if (winner == 1) // If AI won (player lost)
        {
            CommonVariables.AIScore++; // Increments the AI's score
            StatisticsMenuFunctionality.IncrementStatisticValue("Losses"); // Increments the players loss statistic
        }
        else
        {
            Debug.LogWarning("EndGame: Invalid winner input given"); // Outputs a warning to show there's an issue
            winner = 0; // Sets winner to 0 assuming player won
        }

        InstanceReferences.Instance.AdditiveGameMenusInstance.EnableEndScreen(winner); // enables end screen overlay after scores have been updated so they're updated on the end screen
    }

    // Procedure to reset the static game state variables to their default state (time, score, gameactive, paused) - Called when ending/starting a game
    public static void ResetGame(bool fullReset)
    {
        // Sets game state variables to default
        CommonVariables.GameActive = false;
        CommonVariables.Paused = false;
        InstanceReferences.Instance.TimerScriptInstance.ResetTime(); // Calls the ResetTime procedure to set times back to the default 

        if (fullReset) // If full reset is initated it means another game isn't about to be played immediately after. Resets scores
        {
            CommonVariables.AIScore = 0;
            CommonVariables.PlayerScore = 0;
        }
    }

    // Procedure to end a game due to the player exiting either through the pause menu or end game screen with no winner/loser
    public static void FullyEndGame() 
    {
        ResetGame(true); // Calls the reset game function and passes in true to signify it's a full reset
        SceneNavigationFunctions.GoToMainMenu(); // Loads the main menu scene
    }

}

