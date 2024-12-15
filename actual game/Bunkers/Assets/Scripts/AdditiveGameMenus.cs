using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveGameMenus : MonoBehaviour
{
    public static AdditiveGameMenus Instance { get; private set; } // creates a single instance of this class which all other classes can reference

    [SerializeField] GameObject EndMenuUI; // references the end menu gameobject so it can be referenced/enabled/disabled/modified (linked in editor)
    [SerializeField] GameObject PauseMenuUI; // references the pause menu gameobject so it can be referenced/enabled/disabled/modified (linked in editor)

    void Update() // update function runs every frame
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // if escape key's pressed it will either pause or unpause depending on pause status
        {
            if (!CommonVariables.Paused)
            {
                Pause(); // calls pause function
            }
            else
            {
                UnPause(); // calls the unpause function
            }
        }
    }

    // Both menu procedures
    public static void ExitToMenuButton() // procedure to end game
    {
        GeneralBackgroundLogic.ResetGame(true); // calls the ResetGame function to put the game in a new state for if it's started again.
                                                // Passes in true to signify the reset will be full (completely resets the state of the game)
        SceneNavigationFunctions.GoToMainMenu(); // returns user to main menu
    }

    // Pause menu procedures
    public void Pause() // Procedure to pause the game
    {
        PauseMenuUI.SetActive(true); // enables the pause menu object displaying it on screen 
        Time.timeScale = 0f; // sets the speed of time to 0 stopping timers and pausing game 
        CommonVariables.Paused = true; // sets pause status to true
    }

    public void UnPause() // Procedure to unpause the game
    {
        PauseMenuUI.SetActive(false); // disables the pause menu object hiding it
        Time.timeScale = 1f; // sets time to normal
        CommonVariables.Paused = false; // sets pause status to false
    }

    // End menu prodecures
    public void EnableEndScreen()
    {
        EndMenuUI.SetActive(true);
    }

    public void DisableEndScreen() // Method which will disable the end screen. Will be called when a rematch is called for
    {
        EndMenuUI.SetActive(false);
    }

    public static void ExitGameButton() // Method to be called when the exit buttons clicked on the end screen
    {
        Application.Quit(); // Closes the game application down
    }

    public static void RematchButton()
    {
        GeneralBackgroundLogic.ResetGame(false); // Calls reset game function to reset the game for another round (not full as scores need to be kept)
    }


    
}
