using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    // Creates references to the TMP text objects in game for modification in an array
    TMP_Text[] timerTexts = new TMP_Text[2];

    private void Awake()
    {
        timerTexts[0] = InstanceReferences.Instance.PlayerTimerText;
        timerTexts[1] = InstanceReferences.Instance.AITimerText;
    }

    void Start() // Start function to load the saved default times and 
    {
        ResetTime();
        InvokeRepeating("UpdateTime", 1, 1); // Invoke repeating method will call the UpdateTime method every second (starts 1 second after called)
    }



    void UpdateTime() // Update time function is called by InvokeRepeating every second. Updates time
    {
        bool _validated = CommonVariables.GameActive & !CommonVariables.Paused; // Validation to check if game's active and not paused before executing

        if (_validated) // Checks validation before executing
        {
            if (CommonVariables.PlayerTurn) // If it's the players turn (signified by playerturn varaible == true)
            {
                CommonVariables.PlayerTimeLeft = CommonVariables.PlayerTimeLeft - 1;
                UpdateTimerText(CommonVariables.PlayerTimeLeft, 0); // UpdateTimer procedure's called to update the onscreen Player timer. PlayerTimeLeft is inputted + 0 to signify it's the players timer being updated
                Debug.Log($"New player time: {CommonVariables.PlayerTimeLeft}"); // outputs log for testing

                if (CommonVariables.PlayerTimeLeft == 0) // Checks that the player still has time left
                {
                    GeneralBackgroundLogic.EndGame(1); // Calls EndGame function passing in 1 to signify it was the AI who won
                }
            }

            else // If it isn't the players turn it's assumed to be the AI's turn. 
            {
                CommonVariables.AITimeLeft = CommonVariables.AITimeLeft - 1;
                UpdateTimerText(CommonVariables.AITimeLeft, 1); // UpdateTimer procedure's called to update the onscreen AI timer (AITimeLeft + 1 to signify its the AI's turn are inputted)
                Debug.Log($"New player time: {CommonVariables.AITimeLeft}"); // outputs log for testing

                if (CommonVariables.AITimeLeft == 0) // Checks that the AI still has time left
                {
                    GeneralBackgroundLogic.EndGame(0); // Calls EndGame function passing in 0 to signify it was the player who won
                }
            }
        }

    } 
    // End of UpdateTime procedure

    // Function to update the onscreen timer. 
    private void UpdateTimerText(int seconds, int entity) // Takes in how many seconds the timer should have + The entity which timers being updated (0 = Player, 1 = AI)
    {
        if (entity >= 0 && entity <=1) // Validation for entity input to make sure it's within bounds (either 0 or 1 /player or AI)
        {
            var (minutes, remainderSeconds) = SecondsToTime(seconds);
            timerTexts[entity].SetText(minutes + ":" + remainderSeconds);
        }
        else // Else means validation has failed (entity is out of bounds)
        {
            Debug.LogError("UpdateTimer: Entity input is out of bounds (Entity: " +  entity + "). Unable to update timer."); // outputs an error to the log that there has been a wrong input
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
        //Debug.Log("AI loaded time value: " + AITimeLeft);
        //Debug.Log("Player loaded time value : " + PlayerTimeLeft);

        // UpdateTimerText functions then called to update the onscreen text to the values
        UpdateTimerText(CommonVariables.PlayerTimeLeft, 0);
        UpdateTimerText(CommonVariables.AITimeLeft, 1);
    }
} // ------------------------------------------ END OF CLASS ------------------------------------------------------------------
