using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public class ButtonSetting // Object to store values + references for a button setting
{
    [SerializeField] public GameObject[] ButtonGameObjects;
    [SerializeField] public int CurrentButtonRef;
    [SerializeField] public string PlayerPrefName;
}
[Serializable]
public class SliderSetting // Object to store values + references for a slider setting
{
    [SerializeField] public Slider SliderObject;
    [SerializeField] public TextMeshProUGUI SliderValueText;
    [SerializeField] public int DefaultValue;
    [SerializeField] public string PlayerPrefName;
}
[Serializable]
public class InputFieldSetting // Object to store values + references for an input field
{
    [SerializeField] public TMP_InputField InputFieldObject;
    [SerializeField] public bool IsString;
    [SerializeField] public string PlayerPrefName;
}

public class OptionsMenuFunctionality : MonoBehaviour
{
    // Arrays to hold references to different setting objects (buttons, sliders and input fields) - instantiated in unity inspector
    [SerializeField] private ButtonSetting[] _buttonSettings;
    [SerializeField] private SliderSetting[] _sliderSettings;
    [SerializeField] private InputFieldSetting[] _inputFieldSettings;

    private void Start() // Called by unity when scene's loaded
    {
        loadSavedButtonValues(); // Calls LoadSavedButtonValues to highlight the buttons which are the saved player preferences (if there are any)
        loadSavedSliderValues(); // Calls LoadSavedSliderValues to change the slider/s + slider value/s next to it to the stored saved value (if there are any)
        loadSavedInputFieldValues(); // Calls LoadSavedInputFieldValues to load the input fields with saved values
    }


    // ----------------- PROCEDURES TO LOAD SAVED SETTING VALUES + DISPLAY THEM ON SCREEN: -----------------------

   // Procedure to set bold outlined button to the loaded saved button ref
    private void loadSavedButtonValues()
    {
        foreach (ButtonSetting buttonSetting in _buttonSettings) // Goes through every button
        {
            // Changes current button to saved button (if no saved button, 0 is default):
            changeVisuallyActiveButton(buttonSetting, PlayerPrefs.GetInt(buttonSetting.PlayerPrefName, 0));
        }
    }

    // Procedure to set the slider + slider value next to it to saved value
    private void loadSavedSliderValues()
    {
        Debug.Log("Loading saved slider values"); // Outputs that the start functions being executed for testing purposes

        foreach (SliderSetting sliderSetting in _sliderSettings) // Goes through every slider
        {
            int newSliderValue = PlayerPrefs.GetInt(sliderSetting.PlayerPrefName, sliderSetting.DefaultValue); // Sets slider value to player saved value, if no value it sets it to the sliders default value
            sliderSetting.SliderObject.value = newSliderValue;
            sliderSetting.SliderValueText.text = newSliderValue.ToString();
        }
    }

    // Procedure to load input fields with their saved player value
    private void loadSavedInputFieldValues()
    {
        foreach (InputFieldSetting inputFieldSetting in _inputFieldSettings)
        {
            if (inputFieldSetting.IsString) // If it's a string datatype input field
            {
                inputFieldSetting.InputFieldObject.text = PlayerPrefs.GetString(inputFieldSetting.PlayerPrefName); // Sets input fields text to saved value (1 as default if none saved)
            }
            else // If it's an int datatype input field
            {
                inputFieldSetting.InputFieldObject.text = PlayerPrefs.GetInt(inputFieldSetting.PlayerPrefName, 1).ToString(); // Sets input fields text to saved value (1 as default if none saved)
            }
        }
    }

    // ----------------------------------- CHANGE SETTING PROCEDURES: -----------------------------------------

    // ----- Change button procedures -----

    // Procedure to change the active setting (called via unities OnButtonClick for UI buttons)
    public void ChangeButton(int settingAndValueRef) // Has combined setting + value ref due to unity inspector limited to only 1 value passable through a method
    {
        int settingIndexRef = findFirstDigit(settingAndValueRef); // Finds buttonSetting array index through first digit of setting + value ref
        int newValueRef = findLastDigit(settingAndValueRef);
        ButtonSetting buttonSetting = _buttonSettings[settingIndexRef]; // Finds the referenced button setting 
        GameObject newButton = buttonSetting.ButtonGameObjects[newValueRef]; // Finds the referenced button setting (by finding array index by getting last digit of ref)

        PlayerPrefs.SetInt(buttonSetting.PlayerPrefName, newValueRef); // Saves new chosen value to playerprefs via it's index (newValueRef)
        changeVisuallyActiveButton(buttonSetting, newValueRef);

        Debug.Log($"Current active button saved as: {PlayerPrefs.GetInt(buttonSetting.PlayerPrefName)} in PlayerPref: {buttonSetting.PlayerPrefName}");
    }

    // Procedure to change the active outlined button (refactored from changeButton to make it more modular + intercompatible with loadSavedButtonValues)
    private void changeVisuallyActiveButton(ButtonSetting buttonSetting, int newButtonRef)
    {
        // Creates references to the new and old buttons
        GameObject currentButton = buttonSetting.ButtonGameObjects[buttonSetting.CurrentButtonRef];
        GameObject newButton = buttonSetting.ButtonGameObjects[newButtonRef];

        // Changes outline of current button to thin
        Outline obOutlineComponent = currentButton.GetComponent<Outline>();
        obOutlineComponent.effectDistance = new Vector2(-3, -3);

        // Changes outline of new button to thick
        Outline nbOutlineComponent = newButton.GetComponent<Outline>();
        nbOutlineComponent.effectDistance = new Vector2(-5, -5);

        // Sets current button to new button:
        buttonSetting.CurrentButtonRef = newButtonRef;
    }

    // ----- Change slider value procedure -----

    // Procedure to update the slider value saved to what's displayed (called by unity slider when updated)
    public void UpdateSliderValue(int sliderRef) // Method called when volume sliders value's changed (by unity slider)
    {
        SliderSetting sliderSetting = _sliderSettings[sliderRef];
        int sliderValue = (int)sliderSetting.SliderObject.value;

        // Updates saved playerpref value
        PlayerPrefs.SetInt(sliderSetting.PlayerPrefName, sliderValue); // Key is given as sliders playerpref value and value's given as sliders current value

        // Updates value next to the slider
        sliderSetting.SliderValueText.text = sliderValue.ToString();

        Debug.Log($"Slider input saved as: {PlayerPrefs.GetInt(sliderSetting.PlayerPrefName)} in PlayerPref: {sliderSetting.PlayerPrefName}"); // Log for testing
    }

    // ----- Change input field value procedure -----

    public void UpdateInputFieldValue(int inputFieldRef)
    {
        // Creates references to objects based off input field reference
        InputFieldSetting inputFieldSetting = _inputFieldSettings[inputFieldRef];
        TMP_InputField inputFieldObject = inputFieldSetting.InputFieldObject;

        if(inputFieldSetting.IsString) // If input field's string based
        {
            PlayerPrefs.SetString(inputFieldSetting.PlayerPrefName, inputFieldObject.text);
            Debug.Log($"Input saved as string: {PlayerPrefs.GetString(inputFieldSetting.PlayerPrefName)} in PlayerPref: {inputFieldSetting.PlayerPrefName}");
        }
        else // If input fields not string based (int based)
        {
            PlayerPrefs.SetInt(inputFieldSetting.PlayerPrefName, int.Parse(inputFieldObject.text));
            Debug.Log($"Input saved as int: {PlayerPrefs.GetInt(inputFieldSetting.PlayerPrefName)} in PlayerPref: {inputFieldSetting.PlayerPrefName}");
        }
        
    }

    // Procedure to find first digit of a number
    private int findFirstDigit(int number) 
    {
        while (number >= 10)
        {
            number /= 10; 
        }
        return number - 1; // -1 due to int's not allowing 00/01 so 11/12 is used instead as input (then subtracted here)
    }
    // Procedure to find last digit of a number
    private int findLastDigit(int number) 
    {
        return (number % 10 - 1); // -1 due int's not allowing 00/01 so 11/12 is used instead as input (then subtracted here)
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
            // Sets active scene to the gamescene and reenables its audio listener + event system (disabled to stop multiple being active simultaniously)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gamescene"));
            InstanceReferences.Instance.GameSceneAudioListener.enabled = true;
            InstanceReferences.Instance.GameSceneEventSystemParent.SetActive(true);

            // Unloads the option menu
            SceneManager.UnloadSceneAsync("OptionsMenu");
        }
    }

} // ---------------------------------- END OF CLASS ------------------------------------------


