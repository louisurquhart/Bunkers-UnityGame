using TMPro;
using UnityEngine;

public class AdditiveGameMenus : MonoBehaviour
{
    // UI Parent GameObject references
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] GameObject _endMenuUI;

    // End menu TMP component references
    [SerializeField] TMP_Text _endMenuGameOutcomeTMPObject;
    [SerializeField] TMP_Text _endMenuPlayerScoreTMP;
    [SerializeField] TMP_Text _endMenuAIScoreTMP;

    // Array containing win/loss texts 
    private static string[] _winLossText = new string[2]
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
        // MAINTAINANCE - Could add sound effect for pausing/unpausing

        CommonVariables.Paused = !CommonVariables.Paused; // Sets paused status to the opposite of what it is
        _pauseMenuUI.SetActive(CommonVariables.Paused); // Sets the PauseMenuUI active status to whatever the pause status is
    }

    // ------------ BOTH MENU PROCEDURES: ------------
    public static void ExitToMenuButton() // Procedure to end the game + go to main menu
    {
        GeneralBackgroundLogic.FullyEndGame();
    }

    // ------------ END MENU PROCEDURES: -------------

    // Procedure to enable the end screen
    public void EnableEndScreen(int winner) // (0 = Player, 1 = AI)
    {
        InstanceReferences instanceReferences = InstanceReferences.Instance;

        _endMenuUI.SetActive(true); // Sets the EndMenuUI parent gameobject to active
        _endMenuGameOutcomeTMPObject.text = _winLossText[winner]; // Sets the GameOutcome text to the corrosponding win text (If player wins it's "You won", AI wins it's "You lost")

        // Sets the score TMP objects to the actual score values
        _endMenuPlayerScoreTMP.text = CommonVariables.PlayerScore.ToString();
        _endMenuAIScoreTMP.text = CommonVariables.AIScore.ToString();

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
        // MAINTANANCE - Could add sound effect for button press

        GeneralBackgroundLogic.ResetGame(false); // Calls ResetGame function to reset game state variables default. Inputs false to not full reset so scores remain
        GeneralBackgroundLogic.StartGame(); // Calls StartGame to start a new game
    }
}
