using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScripts : MonoBehaviour
{
    public static TimerScripts Instance { get; private set; } // creates a single instance of this class which all other classes can reference to meaning lots of copies dont need to be 
                                                              // created to access the non static methods in it
    public static int PlayerTimeLeft; // creates a float variable to hold the amount of time left for the player
    public static int AITimeLeft; // creates a float variable to hold the amount of time left for the opponent

    public TMP_Text[] timerTexts; // creates an array for reference of the player timers on screen text (0 = player 1 = AI)

    private void Awake()
    {
        Instance = this;
    }


    void Start() // Start function to load the saved default times and 
    {
        ResetTime();
        InvokeRepeating("UpdateTime", 1, 1); // Invoke repeating method will call the UpdateTime method every second (starts 1 second after called)
    }

    // Update function which is called every frame. Updates the timers every frame
    void UpdateTime()
    {
        //Debug.Log("Updating time");

        bool _validated = CommonVariables.GameActive & !CommonVariables.Paused; // Validation to check if code is safe to execute

        if (_validated) // Checks validation before executing
        {
            if (CommonVariables.PlayerTurn) // if it's the players turn (signified by playerturn varaible == true)
            {
                PlayerTimeLeft = PlayerTimeLeft - 1;
                UpdateTimerText(PlayerTimeLeft, 0); // UpdateTimer procedure's called to update the onscreen Player timer. PlayerTimeLeft is inputted + 0 to signify it's the players timer being updated
                //Debug.Log("New player time: " + PlayerTimeLeft); // outputs log for testing

                if (PlayerTimeLeft == 0) // Checks that the player still has time left
                {
                    GeneralBackgroundLogic.EndGame(1); // Calls EndGame function passing in 1 to signify it was the AI who won
                }
            }

            else // if it isn't the players turn it's assumed to be the AI's turn. 
            {
                AITimeLeft = AITimeLeft - 1;
                UpdateTimerText(AITimeLeft, 1); // UpdateTimer procedure's called to update the onscreen AI timer (AITimeLeft + 1 to signify its the AI's turn are inputted)
                //Debug.Log("New AI time: " + PlayerTimeLeft); // outputs log for testing

                if (AITimeLeft == 0) // Checks that the AI still has time left
                {
                    GeneralBackgroundLogic.EndGame(0); // Calls EndGame function passing in 0 to signify it was the player who won
                }
            }
        }
    } 
    // End of UpdateTime procedure

    // Function to update the onscreen timer. 
    public void UpdateTimerText(float seconds, int entity) // Takes in how many seconds the timer should have + The entity which timers being updated (0 = Player, 1 = AI)
    {
        if (entity >= 0 && entity <=1) // validation for entity input to make sure it's either 0 or 1 (player or AI)
        {
            var (minutes, remainderSeconds) = SecondsToTime(PlayerTimeLeft);
            timerTexts[entity].SetText(minutes + ":" + remainderSeconds);
        }
        else // else means validation has failed (entity is out of bounds)
        {
            Debug.LogError("UpdateTimer: Entity input is out of bounds (Entity: " +  entity + "). Unable to update timer."); // outputs an error to the log that there has been a wrong input
        }
    }

    // SecondsToTime function where seconds are converted into minutes + remainder seconds. Seperate function for easier maintance and addition of new features.
    public static (string sMinutes, string sRemainderSeconds) SecondsToTime(int seconds) // Input: Seconds -> Output: Minutes + ReaminderSeconds
    {
        
        if (seconds >= 0 && seconds < 3600) // validation to check inputted time's within bounds
        {
            int minutes = seconds / 60; // divdes the int seconds by 60 to find out how many minutes there are in the amount of seconds
            int remainderSeconds = seconds % 60; // uses modulus to find the remainder seconds
            //Debug.Log("Minutes: " + minutes + " Seconds: " + remainderSeconds);
            string sMinutes = minutes.ToString();
            string sRemainderSeconds = remainderSeconds.ToString("D2");
            return (sMinutes, sRemainderSeconds); // returns minutes + remaindingseconds
        }
        else // if validation fails
        {
            //Debug.LogError("SecondsToTime: Inputted seconds are out of bounds, Convertion failed."); // outputs error to console for testing/debugging
            return ("7", "30"); // returns the default time
        }
    }

    public void ResetTime()
    {
        // The Player and AI Time variables are set to either the saved times or a default time
        PlayerTimeLeft = PlayerPrefs.GetInt("TimeLeft", 450); // loads the Player time value set in options with playerprefs and sets PlayerTimeLeft variable to it (if no value the default's set to 450 seconds)
        AITimeLeft = PlayerPrefs.GetInt("TimeLeft", 450); // loads the AI time value set in options and sets the AITimeLeft variable to it (if no value the default's set to 450 seconds)
        //Debug.Log("AI loaded time value: " + AITimeLeft);
        //Debug.Log("Player loaded time value : " + PlayerTimeLeft);

        // UpdateTimerText functions then called to update the onscreen text to the values
        UpdateTimerText(PlayerTimeLeft, 0);
        UpdateTimerText(AITimeLeft, 1);
    }



} // ------------------------------------------ END OF CLASS ------------------------------------------------------------------
