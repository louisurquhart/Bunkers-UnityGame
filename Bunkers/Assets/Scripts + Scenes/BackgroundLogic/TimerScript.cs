using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    // Creates references to the TMP text objects in game for modification in an array
    [SerializeField] TMP_Text[] timerTexts = new TMP_Text[2];

    void Start() // Start function to load the saved default times and 
    {
        ResetTime(); // Resets time just incase it hasn't already been done (redundant)
        InvokeRepeating("updateTime", 1, 1); // Invoke repeating method will call the UpdateTime method every second (starts 1 second after called)
    }

    private void updateTime() // Update time function is called by InvokeRepeating every second. Updates time
    {
        bool _validated = CommonVariables.GameActive & !CommonVariables.Paused; // Validation to check if game's active and not paused before executing

        StatisticsMenuFunctionality.IncrementStatisticValue("MinutesSpentInGame");

        if (_validated) // Checks validation before executing
        {
            PlayerPrefs.SetInt("TimeSpentInGame", PlayerPrefs.GetInt("TimeSpentInGame" + 1)); // Increments the PlayerPref statistic "TimeSpentInGame" 

            if (CommonVariables.PlayerTurn) // If it's the players turn (signified by playerturn varaible == true)
            {
                CommonVariables.PlayerTimeLeft = CommonVariables.PlayerTimeLeft - 1; // Decrements players timeleft
                updateTimerText(CommonVariables.PlayerTimeLeft, 0); // UpdateTimer procedure's called to update the onscreen Player timer. PlayerTimeLeft is inputted + 0 to signify it's the players timer being updated

                if (CommonVariables.PlayerTimeLeft == 0) // Checks that the player still has time left
                {
                    GeneralBackgroundLogic.EndGame(1); // Calls EndGame function passing in 1 to signify it was the AI who won
                }
            }

            else // If it isn't the players turn it's assumed to be the AI's turn. 
            {
                CommonVariables.AITimeLeft = CommonVariables.AITimeLeft - 1; // Decrements the AI's time left
                updateTimerText(CommonVariables.AITimeLeft, 1); // UpdateTimerText procedure's called to update the onscreen AI timer (AITimeLeft + 1 to signify its the AI's turn are inputted)

                if (CommonVariables.AITimeLeft == 0) // Checks that the AI still has time left
                {
                    GeneralBackgroundLogic.EndGame(0); // Calls EndGame function passing in 0 to signify it was the player who won
                }
            }
        }
    }

    // Procedure to update the onscreen timer. 
    private void updateTimerText(int seconds, int entity) // Takes in how many seconds the timer should have + The entity which timers being updated (0 = Player, 1 = AI)
    {
        if (entity >= 0 && entity <= 1) // Validation for entity input to make sure it's within bounds (either 0 or 1 /player or AI)
        {
            var (minutes, remainderSeconds) = SecondsToTime(seconds);
            timerTexts[entity].SetText(minutes + ":" + remainderSeconds);
        }
        else // Else means validation has failed (entity is out of bounds)
        {
            Debug.LogError("UpdateTimer: Entity input is out of bounds (Entity: " + entity + "). Unable to update timer."); // outputs an error to the log that there has been a wrong input
        }
    }

    // SecondsToTime function where seconds are converted into minutes + remainder seconds. Seperate function for easier maintance and addition of new features.
    private static (string sMinutes, string sRemainderSeconds) SecondsToTime(int seconds) // Input: Seconds -> Output: Minutes + ReaminderSeconds
    {

        if (seconds >= 0 && seconds <= 3600) // Validates seconds are within bounds (0 - 3
        {
            int minutes = seconds / 60; // divdes the int seconds by 60 to find out how many minutes there are in the amount of seconds
            int remainderSeconds = seconds % 60; // Uses modulus to find the remainding seconds
            string sMinutes = minutes.ToString();
            string sRemainderSeconds = remainderSeconds.ToString("D2"); // Converts the remainding seconds to a string with "D2" signifying only 2 digits (removes any decimals)
            return (sMinutes, sRemainderSeconds); // Returns minutes + remaindingseconds
        }
        else // If validation fails
        {
            Debug.LogError("SecondsToTime: Inputted seconds are out of bounds, Convertion failed."); // outputs error to console for testing/debugging
            return ("7", "30"); // returns the default time
        }
    }

    public void ResetTime()
    {
        // The Player and AI Time variables are set to either the saved times or a default time
        CommonVariables.PlayerTimeLeft = PlayerPrefs.GetInt("TimeLeft", 450); // loads the Player time value set in options with playerprefs and sets PlayerTimeLeft variable to it (if no value the default's set to 450 seconds)
        CommonVariables.AITimeLeft = PlayerPrefs.GetInt("TimeLeft", 450); // loads the AI time value set in options and sets the AITimeLeft variable to it (if no value the default's set to 450 seconds)

        // UpdateTimerText functions then called to update the onscreen text to the values
        updateTimerText(CommonVariables.PlayerTimeLeft, 0);
        updateTimerText(CommonVariables.AITimeLeft, 1);
    }
} // ------------------------------------------ END OF CLASS ------------------------------------------------------------------
