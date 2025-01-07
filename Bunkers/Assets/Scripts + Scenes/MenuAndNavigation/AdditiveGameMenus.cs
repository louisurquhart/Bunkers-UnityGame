using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveGameMenus : MonoBehaviour
{
    // UI Parent GameObject references
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject endMenuUI;

    // End menu TMP component references
    [SerializeField] TMP_Text endMenuGameOutcomeTMPObject;
    [SerializeField] TMP_Text endMenuPlayerScoreTMP;
    [SerializeField] TMP_Text endMenuAIScoreTMP;

    // Array containing win/loss texts 
    private static string[] winLossText = new string[2]
    {
        "You won", // pos 0 - player wins
        "You lost" // pos 1 - ai wins
    };


    void Update() // Update function runs every frame
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // If escape key's pressed it will either pause or unpause depending on pause status
        {
            TogglePauseStatus(); // Calls TogglePauseStatus to change the games paused state
        }
    }

    // ------------ PAUSE MENU PROCEDURE:  -----------
    public void TogglePauseStatus()
    {
        CommonVariables.Paused = !CommonVariables.Paused; // Sets paused status to the opposite of what it is
        pauseMenuUI.SetActive(CommonVariables.Paused); // Sets the PauseMenuUI active status to whatever the pause status is
    }

    // ------------ BOTH MENU PROCEDURES: ------------
    public static void ExitToMenuButton() // Procedure to end the game + go to main menu
    {
        GeneralBackgroundLogic.FullyEndGame(); // 
    }

    // ------------ END MENU PROCEDURES: -------------

    // Procedure to enable the end screen
    public void EnableEndScreen(int winner) // (0 = Player, 1 = AI)
    {
        InstanceReferences instanceReferences = InstanceReferences.Instance;

        endMenuUI.SetActive(true); // Sets the EndMenuUI parent gameobject to active
        endMenuGameOutcomeTMPObject.text = winLossText[winner]; // Sets the GameOutcome text to the corrosponding win text (If player wins it's "You won", AI wins it's "You lost")

        // Sets the score TMP objects to the actual score values
        endMenuPlayerScoreTMP.text = CommonVariables.PlayerScore.ToString();
        endMenuAIScoreTMP.text = CommonVariables.AIScore.ToString();

    }

    // Procedure to exit the game
    public void ExitToDesktop() 
    {
        StatisticsMenuFunctionality.IncrementStatisticValue("MidgameQuits"); // Updates statistics

        Application.Quit(); // Closes the game application down
    }

    // Procedure to initiate a rematch
    public static void Rematch() // function to play another game (when one's lost)
    {
        GeneralBackgroundLogic.ResetGame(false); // Calls ResetGame function to reset game state variables default. Inputs false to not full reset so scores remain
        GeneralBackgroundLogic.StartGame();
    }
}
