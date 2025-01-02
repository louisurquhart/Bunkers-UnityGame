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
        Console.WriteLine("Attempting to start a singleplayer game");
        ResetGame(false); // Soft resets the game for redundancy (should already be done)
        SceneManager.LoadScene("Gamescene"); // loads the gamescene
        CommonVariables.Paused = false; // makes sure the game's unpaused
        CommonVariables.GameActive = true; // sets gameactive == true for other methods and functions to work properly (eg timers)
    }

    public static bool HasGameEnded() // Procedure to check if a game has ended and the entity type inputted (0 = player, 1 = AI)
    {
        if (CommonVariables.PlayerAliveBunkerCount <= 0)
        {
            Debug.Log($"<b>{CommonVariables.DebugFormat[1]}HasGameEnded: Determined AI has won (PlayerAliveBunkerCount: {CommonVariables.PlayerAliveBunkerCount})");
            EndGame(1); // End game function. 1 is passed in to signify the AI won
            return true;

        }
        else if(CommonVariables.AIAliveBunkerCount <= 0)
        {
            Debug.Log($"<b>{CommonVariables.DebugFormat[0]}HasGameEnded: Determined player has won (AIAliveBunkerCount: {CommonVariables.AIAliveBunkerCount})");
            EndGame(0); // End game function. 0 is passed in to signify the player won
            return true;

        }
        else // If neither are true the game can't have ended so false is returned
        {
            return false;
        }
    }

    public static void ChangeTurn() // Procedure to switch turns between AI and the player
    {
        CommonVariables.PlayerTurn = !CommonVariables.PlayerTurn; // Changes turn to whatever the current turn isn't
        //Debug.Log($"PlayerTurn changed to: {CommonVariables.PlayerTurn}"); // Outputs a message to the debug console for testing and debugging

        // Change turn sound effect will be added in final iteration

        if (CommonVariables.PlayerTurn) // If the new turn's the players turn
        {
            Debug.Log($"<b><color=white>PLAYER TURN:");
            // Needs to prompt player it's their turn
        }
        else // If the new turn value's the AI turn
        {
            Debug.Log($"<b><color=white>AI TURN:");
            AILogic.InitiateAITurn();
        }
    }

    public static void EndGame(int winner) // Procedure to end a game due to someone winning through game logic (0 = player, 1 = AI)
    {
        CommonVariables.GameActive = false; // Sets GameActive to false

        // Statistics update
        if(winner == 0) // If player won
        {
            CommonVariables.PlayerScore++;
            PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1); // Adds 1 to players win count in statistics
        }
        else // If player lost
        {
            CommonVariables.AIScore++;
            PlayerPrefs.SetInt("Losses", PlayerPrefs.GetInt("Losses") + 1); // Adds 1 to players loss count in statistics
        }

        AdditiveGameMenus.EnableEndScreen(winner); // enables end screen overlay after scores have been updated so they're updated on the end screen

    } // End of EndGame method

    public static void FullyEndGame() // Procedure to end a game due to the player exiting either through the pause menu or end game screen. 
    {
        // maybe update statistics for games quit
        SceneNavigationFunctions.GoToMainMenu();
        ResetGame(true); // Calls the reset game function and passes in true to signify it's a full reset
    }


    public static void ResetGame(bool fullReset)
    {
        // Sets common variables to default
        InstanceReferences.Instance.TimerScriptInstance.ResetTime(); // Calls the ResetTime procedure to set times back to the default 
        CommonVariables.GameActive = false;
        CommonVariables.Paused = false;

        if (fullReset) // If full reset is initated it means another game isn't about to be played immediately after. Resets scores
        {
            CommonVariables.AIScore = 0;
            CommonVariables.PlayerScore = 0;
        }
    }

    public static void TogglePauseStatus() // Procedure to pause the game
    {
        CommonVariables.Paused = !CommonVariables.Paused; // Sets paused status to the opposite of what it is
        InstanceReferences.Instance.PauseMenuUI.SetActive(CommonVariables.Paused); // Sets the PauseMenuUI active status to whatever the pause status is

        if (CommonVariables.Paused)
        {
            Time.timeScale = 0f; // Sets the speed of time to 0 stopping timers and pausing game 
        }
        else
        {
            Time.timeScale = 1f;
        }
    }






} // ------------------ End of GeneralBackgroundGameLogic class ------------------

