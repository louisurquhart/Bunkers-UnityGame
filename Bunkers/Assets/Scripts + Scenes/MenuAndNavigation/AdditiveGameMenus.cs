using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveGameMenus : MonoBehaviour
{
    static string[] winLossText = new string[2]
    {
        "You won", // pos 0 - player wins
        "You lost" // pos 1 - ai wins
    };


    void Update() // Update function runs every frame
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // If escape key's pressed it will either pause or unpause depending on pause status
        {
            GeneralBackgroundLogic.TogglePauseStatus(); // Calls TogglePauseStatus to change the games paused state
        }
    }

    public static void PauseButton()
    {
        GeneralBackgroundLogic.TogglePauseStatus();
    }

    // Both menu procedures
    public static void ExitToMenuButton() // Procedure to end the game + go to main menu
    {
        GeneralBackgroundLogic.FullyEndGame();
    }

    // End menu prodecures
    public static void EnableEndScreen(int winner) // (0 = Player, 1 = AI)
    {
        InstanceReferences instanceReferences = InstanceReferences.Instance;

        instanceReferences.EndMenuUI.SetActive(true); // Sets the EndMenuUI parent gameobject to active
        instanceReferences.EndMenuGameOutcomeTMPObject.text = winLossText[winner]; // Sets the GameOutcome text to the corrosponding win text (If player wins it's "You won", AI wins it's "You lost")

        // Sets the score TMP objects to the actual score values
        instanceReferences.EndMenuPlayerScoreTMP.text = CommonVariables.PlayerScore.ToString();
        instanceReferences.EndMenuAIScoreTMP.text = CommonVariables.AIScore.ToString();

    }

    public static void ExitToDesktop() // Procedure to exit to desktop 
    {
        Application.Quit(); // Closes the game application down
    }

    public static void Rematch() // function to play another game (when one's lost)
    {
        GeneralBackgroundLogic.ResetGame(false); // Calls ResetGame function to reset game state variables default. Inputs false to not full reset so scores remain
        GeneralBackgroundLogic.StartGame();
    }
}
