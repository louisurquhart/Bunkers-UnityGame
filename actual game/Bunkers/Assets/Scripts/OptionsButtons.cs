using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionsButtons : MonoBehaviour
{
    // ----------------------------------  START OF CLASS  ------------------------------------------
    public GameObject OptionsEventManager;
    public GameObject OptionsAudioListener;

    private void Start()
    {
        if (CommonVariables.GameActive == true) // if there's a game active it means it was loaded additively from pause menu
        {
            OptionsEventManager.SetActive(false); // so the options menu event manager's disabled
            OptionsAudioListener.SetActive(false);                                      
            // PROBABLY SHOULD TRY HAVE A COMMON SCENE WITH AN EVENT MANAGER NEEDS RESEARCH 
        }
    }

    // ------------------------------------ BACK BUTTON --------------------------------------------
    public void ExitButton()
    {
        if (CommonVariables.GameActive == false) // checks if a game's currently active
        {
            SceneManager.LoadScene("MainMenu"); // if no games active it returns the user to the main menu
        }
        else
        {
            SceneManager.UnloadSceneAsync("OptionsMenu"); // otherwise it unloads the option menu because the users mid game 
        }
    }

    // --------------------------------- ON CLICK AI SETTINGS -----------------------------------------

    // Difficulty
    public void ChangeDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty); // Saves the chosen difficulty to player prefs (the difficulty variables passed via the onclick button function)
    }

    // Special strike enabled or disabled

    public void SpecialStrikesStatus(int specialstrikevalue)
    {
        PlayerPrefs.SetInt("SpecialStrikeStatus", specialstrikevalue); // 0 = FALSE and 1 = TRUE because playerprefs can't use boolean 
        Console.WriteLine("Special strike saved status: ", PlayerPrefs.GetInt("SpecialStrikeStatus", 2)); // outputs saved status of value for testing. 2 indicates no value meaning error
    }

    // ----------------------------- ON CLICK STATISTICS SETTINGS -------------------------------

    // enable/disable statistic recording

    public void StatisticRecordingStatus(int statisticrecordingstatus)
    {
        PlayerPrefs.SetInt("StatisticRecording", statisticrecordingstatus); // 0 = FALSE and 1 = TRUE because playerprefs can't use boolean
        Console.WriteLine("Statistic recording saved status: ", PlayerPrefs.GetInt("StatisticRecording", 2)); // outputs statistic recording status for testing. 2 indicates no value meaning error
    }

    // type of statistics recorded
    public void StatisticRecordingType(string statisticsrecorded)
    {
        PlayerPrefs.SetString("StatisticRecordingType", statisticsrecorded);
        Console.WriteLine("Statistic recording type saved status: ", PlayerPrefs.GetString("StatisticRecordingType", "Null")); // outputs saved status of value for testing. null indicates no value meaning error
    }

    // ----------------------------- ON CLICK GENERAL SETTINGS -------------------------------

    // volume slider in another script



















    // ----------------------------- STATISTICS SETTINGS -------------------------------


} // ---------------------------------- END OF CLASS ------------------------------------------
