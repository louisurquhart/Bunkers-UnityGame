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
        SceneManager.LoadScene("Gamescene"); // loads the gamescene

        // Move grid generation function calls to here instead of them being done on start
        // After grid generation


        CommonVariables.Paused = false; // makes sure the game's unpaused
        CommonVariables.GameActive = true; // sets gameactive == true for other methods and functions to work properly (eg timers)
    }

    // NEEDS WORK
    public static bool HasGameEnded() // Procedure to check if a game has ended and the entity type inputted (0 = player, 1 = AI)
    {
        if (CommonVariables.PlayerBunkerCount == 0)
        {
            CommonVariables.PlayerScore += 1; // Updates the players win count in common variables
            EndGame(0); // End game function. 0 is passed in to signify the player won
            return true;

        }
        else if (CommonVariables.PlayerBunkerCount > 0)
        {
            return false;
        }
        else
        {
            Debug.LogError("ERROR - HasGameEnded: Player has less than 0 ships, Ending game"); // error message for testing for a case in which somehow the players ship count's negative
            EndGame(1); // End game function. 1 is passed in to signify the AI won
            return true;
        }
    }

    public static void ChangeTurn() // Procedure to switch turns between AI and the player
    {
        CommonVariables.PlayerTurn = !CommonVariables.PlayerTurn; // Changes turn to whatever the current turn isn't
        Debug.Log($"PlayerTurn changed to: {CommonVariables.PlayerTurn}"); // Outputs a message to the debug console for testing and debugging
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

            AdditiveGameMenus.ToggleEndScreen(); // enables end screen overlay after scores have been updated so they're updated on the end screen

            // need to update statistics
        }
    } // End of EndGame method

    public static void ForceEndGame() // Procedure to end a game due to the player exiting either through the pause menu or end game screen. 
    {
        // maybe update statistics for games quit
        SceneNavigationFunctions.GoToMainMenu();
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

    public static Tile GenerateRandomTile(GridManager gridManager) // Method to return a random tile
    {
        int randomRow = UnityEngine.Random.Range(0, 10); // Generates a random row
        int randomColumn = UnityEngine.Random.Range(0, 10); // Generates a random column
        Tile randomTile = gridManager.Grid[randomRow, randomColumn]; // Finds the position of the random row + column combined and the tile located at it (through the referenced gridmanager)

        return randomTile; // Returns the random tile
    }

} // ------------------ End of GeneralBackgroundGameLogic class ------------------

