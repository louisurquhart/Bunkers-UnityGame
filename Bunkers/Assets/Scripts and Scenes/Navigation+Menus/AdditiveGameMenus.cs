using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveGameMenus : MonoBehaviour
{

    void Update() // Update function runs every frame
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // If escape key's pressed it will either pause or unpause depending on pause status
        {
            GeneralBackgroundLogic.TogglePauseStatus(); // Calls TogglePauseStatus to change the games paused state
        }
    }

    // Both menu procedures
    public static void ExitToMenuButton() // Procedure to end the game + go to main menu
    {
        GeneralBackgroundLogic.ResetGame(true); // calls the ResetGame function to put the game in a new state for if it's started again.                                    // Passes in true to signify the reset will be full (completely resets the state of the game)
        SceneNavigationFunctions.GoToMainMenu(); // returns user to main menu
    }

    // End menu prodecures
    public static void ToggleEndScreen() // Procedure to toggle the end screen on/off
    {
        GameObject endMenuUI = InstanceReferences.Instance.EndMenuUI; // Creates a reference to the EndMenuUI
        InstanceReferences.Instance.EndMenuUI.SetActive(!(endMenuUI.activeSelf)); // Sets the EndMenuUI active state to whatever it isn't
    }

    public static void ExitToDesktop() // Procedure to exit to desktop 
    {
        Application.Quit(); // Closes the game application down
    }

    public static void RematchButton()
    {
        GeneralBackgroundLogic.ResetGame(false); // Calls reset game function to reset the game for another round (not full as scores need to be kept)
    }
}
