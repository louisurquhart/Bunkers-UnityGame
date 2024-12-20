using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionsMenuFunctionality : MonoBehaviour
{ 
    private void Start() // Called by unity when scene is loaded
    {
        Debug.Log("Loading saved volume value"); // Outputs that the start functions being executed for testing purposes
        float initialvolume = PlayerPrefs.GetFloat("GameVolume", 50); // Sets volume value to player saved value, if no value it sets it to default of 50
        VolSliderText.text = initialvolume.ToString();  // Sets the text next to the slider to the loaded volume value
        VolSlider.value = initialvolume; // Sets the actual slider to the loaded volume value
    }

    // ------------------------------------ BACK BUTTON --------------------------------------------
    public void ExitButton()
    {
        if (!CommonVariables.GameActive) // If a game isn't currently active
        {
            SceneManager.LoadScene("MainMenu"); // If there's no game active it returns the user to the main menu.
        }
        else // Otherwise if a game is currently active
        {
            SceneManager.UnloadSceneAsync("OptionsMenu"); // If a game's active it only unloads the option menu because the users in the middle of the game and the scene is overlayed ontop of it.
        }
    }

    // ------------------------------------- AI SETTINGS -----------------------------------------

    // Difficulty
    public void ChangeDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty); // Saves the chosen difficulty to player prefs (the difficulty variables passed via the onclick button function)
        Debug.Log($"Difficulty saved status: {PlayerPrefs.GetInt("Difficulty", 2)}"); // Outputs saved status. If no value has been saved, 2 is outputted to indicate there's an error
    }

    // Special strike enabled or disabled

    public void SpecialStrikesStatus(int specialstrikevalue)
    {
        PlayerPrefs.SetInt("SpecialStrikeStatus", specialstrikevalue); // Saves the value inputted by the player for SpecialStrikeStatus. 0 = False and 1 = True because playerprefs can't use boolean 
        Debug.Log($"Special strike saved status: {PlayerPrefs.GetInt("SpecialStrikeStatus", 2)}"); // Outputs saved status. If no value has been saved, 2 is outputted to indicate there's an error
    }

    // ---------------------------------- STATISTICS SETTINGS -------------------------------

    // Enable/disable statistic recording

    public void StatisticRecordingStatus(int statisticrecordingstatus)
    {
        PlayerPrefs.SetInt("StatisticRecording", statisticrecordingstatus); // Saves the value inputted for statistics recording status. 0 = FALSE and 1 = TRUE because playerprefs can't use boolean
        Debug.Log($"Statistic recording saved status: {PlayerPrefs.GetInt("StatisticRecording", 2)}"); // Outputs saved status. If no value has been saved, 2 is outputted to indicate there's an error
    }

    // Type of statistics recorded
    public void StatisticRecordingType(string statisticsrecorded)
    {
        PlayerPrefs.SetString("StatisticRecordingType", statisticsrecorded); // Saves the value inputted for statistics recording type. 0 = FALSE and 1 = TRUE because playerprefs can't use boolean. 
        Debug.Log($"Statistic recording type saved status: {PlayerPrefs.GetString("StatisticRecordingType", "Null")}"); // Outputs saved status. If no value has been saved, 2 is outputted to indicate there's an error
    }

    // -------------------------------- GENERAL SETTINGS -------------------------------

    [SerializeField] Slider VolSlider; // Creates a reference for the volume slider in the options menu. It will be referenced via the inspector.
    [SerializeField] TextMeshProUGUI VolSliderText; // Creates a reference for the volume slider in the options menu. It will be referenced via the inspector.


    public void VolumeChange() // Method called when volume sliders value's changed
    {
        VolSliderText.text = VolSlider.value.ToString(); // Changes text next to volume slider to the value of the volume slider
        AudioListener.volume = VolSlider.value / 100; // Sets the ingame volume to the saved value
        PlayerPrefs.SetFloat("GameVolume", VolSlider.value); // Saves this volume to playerprefs so it can be loaded next time the game's opened
        Debug.Log($"Current volume is: {VolSlider.value}"); // Outputs the new volume value to the debug log for testing
    }

} // ---------------------------------- END OF CLASS ------------------------------------------
