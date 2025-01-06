using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class OptionsMenuFunctionality : MonoBehaviour
{
    private static int _arrayCount = 5;

    Array[] gameObjectArrays = new Array[_arrayCount];
    String[] playerPrefName = new string[_arrayCount];
    GameObject[] currentButtonsArray = new GameObject[_arrayCount];

    // References to the button/slider GameObjects for modification

    // Difficulty button references
    [SerializeField] GameObject[] _difficultyButtonReferences = new GameObject[3];

    // Special strike button references:
    [SerializeField] GameObject[] _specialStrikeStatusButtonReferences = new GameObject[2];

    // Statistic recording button references:
    [SerializeField] GameObject[] _statisticRecordingStatusButtonReferences = new GameObject[2];

    // Type of statistics recorded button references:
    [SerializeField] GameObject[] _statisticRecordingTypeButtonReferences = new GameObject[3];

    // Volume slider references:
    [SerializeField] GameObject[] _volumeSliderGameObjectReferences = new GameObject[2];

    // Awake method adds all the arrays to the main array in runtime
    private void Awake() // Awake method is called immediately after class initiailization
    {
        // Adds all the sub arrays to the array of arrays + the playerprefs string value reference to the other array (can't be multidimensional as different types)
        gameObjectArrays[0] = _difficultyButtonReferences;
        playerPrefName[0] = "Difficulty";
        currentButtonsArray[0] = _difficultyButtonReferences[0];

        gameObjectArrays[1] = _specialStrikeStatusButtonReferences;
        playerPrefName[1] = "SpecialStrikeStatus";
        currentButtonsArray[1] = _specialStrikeStatusButtonReferences[0];

        gameObjectArrays[2] = _statisticRecordingStatusButtonReferences;
        playerPrefName[2] = "StatisticRecordingStatus";
        currentButtonsArray[2] = _statisticRecordingStatusButtonReferences[0];

        gameObjectArrays[3] = _statisticRecordingTypeButtonReferences;
        playerPrefName[3] = "StatisticRecordingType";
        currentButtonsArray[3] = _statisticRecordingTypeButtonReferences[0];
    }

    private void Start() // Called by unity when scene is loaded
    {
        loadSavedButtonValues(); // Calls LoadSavedButtonValues to highlight the buttons which are the saved player preferences
        loadSavedSliderValues();
    }

    // Method to set the volumeslider + number next to it to the saved volume value.
    private void loadSavedSliderValues()
    {
        //Debug.Log("Loading saved volume value"); // Outputs that the start functions being executed for testing purposes

        // Creates references to the specific GameObjects needed
        Slider volSlider = _volumeSliderGameObjectReferences[0].GetComponent<Slider>();
        TMP_Text volNumber = _volumeSliderGameObjectReferences[1].GetComponent<TMP_Text>();

        // Loads the saved value from playerprefs (if no saved value, 50 is loaded as default
        float loadedVolume = PlayerPrefs.GetFloat("GameVolume", 50); // Sets volume value to player saved value, if no value it sets it to default of 50

        // Sets the volume slider + number to the loaded value
        volNumber.text = loadedVolume.ToString();  // Sets the text next to the slider to the loaded volume value
        volSlider.value = loadedVolume; // Sets the actual slider to the loaded volume value
    }

    // ------------------------------ HIGHLIGHT SAVED BUTTONS PROCEDURE (called on start) -----------------------------------------
    private void loadSavedButtonValues()  // Called to highlight the option buttons which values were loaded from the users saved playerpreferences (if no values saved, the default value is highlighted)
    {
        for (int i = 0; i < _arrayCount; i++) // For loop goes through all the items in gameObjectArray
        {
            Array currentArray = gameObjectArrays[i]; // Reference is created for current array in question for easier maintainability + readability
            Debug.Log($"currentArray is now set to array {i}");

            int savedValue = PlayerPrefs.GetInt(playerPrefName[i], -1); // Sets savedValue to the saved value stored in playerprefs (loads default of -1 if no saved value)

            if (savedValue == -1) // If there's no saved value (default of -1 has been loaded), it will skip this iteration of the loop as nothing needs changing
            {
                continue; // Skips the rest of the code and reloops as nothing needs changing because value's already default
            }
            else if (savedValue <= 3) // Otherwise if value isn't default + is within bounds (<= 3) it will change the visual properties of the corrosponding button
            {
                Debug.Log($"Changing setting reference: {i} to value: {savedValue}");
                // ---- Changes outline width of the designated button: ----
                GameObject newButton = (GameObject)currentArray.GetValue(savedValue); // Finds the corropsonding button to the loaded value and casts it to a gameobject and sets a reference to it
                changeOutline(i, newButton);
            }
            else // If value's out of bounds
            {
                Debug.LogError($"Validation failiure for loadSavedButtonValues. Loaded saved value is out of bounds. Loaded value == {savedValue}"); // Outputs an error to the debug log that the code couldn't executed due to out of bounds value
            }
        }
    }

    private void changeOutline(int settingRef, GameObject newButton) // Method to change the thickness of a gameObject
    {
        GameObject currentButtonValue = currentButtonsArray[settingRef];

        Outline obOutlineComponent = currentButtonValue.GetComponent<Outline>();
        obOutlineComponent.effectDistance = new Vector2(-3, -3);

        currentButtonsArray[settingRef] = newButton;

        Outline nbOutlineComponent = newButton.GetComponent<Outline>();
        nbOutlineComponent.effectDistance = new Vector2(-5, -5);
    }


    // -------------------------------------- CHANGE BUTTON PROCEDURE -----------------------------------------
    public void ChangeSetting(int settingAndValueRef)
    {
        int settingNumberRef = findFirstDigit(settingAndValueRef) - 1;
        int value = findLastDigit(settingAndValueRef) - 1;

        string setting = playerPrefName[settingNumberRef];

        PlayerPrefs.SetInt(setting, value); // Saves the chosen difficulty to player prefs (the difficulty variables passed via the onclick button function)

        changeOutline(settingNumberRef, (GameObject)gameObjectArrays[settingNumberRef].GetValue(value));

        Debug.Log($"Setting: {setting} changed to: {value}.");
    }

    private int findFirstDigit(int number)
    {
        while (number >= 10)
        {
            number /= 10;
        }
        return number;
    }
    private int findLastDigit(int number)
    {
        return (number % 10);
    }

    // -------------------------------------- CHANGE SLIDER PRODCEDURE -----------------------------------------

    // Change the volume
    public void ChangeVolume() // Method called when volume sliders value's changed
    {
        // Creates references to the specific components needed
        Slider volSlider = _volumeSliderGameObjectReferences[0].GetComponent<Slider>();
        TMP_Text volNumber = _volumeSliderGameObjectReferences[1].GetComponent<TMP_Text>();

        // Changes the volume + text next to the volume slided to the value of the volume slider. (called when volume slider is moved)
        volNumber.text = volSlider.value.ToString(); // Changes text next to volume slider to the value of the volume slider
        AudioListener.volume = volSlider.value / 100; // Sets the ingame volume to the saved value
        PlayerPrefs.SetFloat("GameVolume", volSlider.value); // Saves this volume to playerprefs so it can be loaded next time the game's opened
        Debug.Log($"Current volume is: {volSlider.value}"); // Outputs the new volume value to the debug log for testing
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
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gamescene"));
            InstanceReferences.Instance.GameSceneAudioListener.enabled = true;
            InstanceReferences.Instance.GameSceneEventSystemParent.SetActive(true);
            SceneManager.UnloadSceneAsync("OptionsMenu"); // If a game's active it only unloads the option menu because the users in the middle of the game
                                                          // and the scene is overlayed ontop of it.
        }
    }

} // ---------------------------------- END OF CLASS ------------------------------------------
