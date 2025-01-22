using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public class ButtonSetting 
{
    [SerializeField] public GameObject[] ButtonGameObjects;
    [SerializeField] public int CurrentButtonRef;
    [SerializeField] public string PlayerPrefName;
}
[Serializable]
public class SliderSetting
{
    [SerializeField] public Slider SliderObject;
    [SerializeField] public TextMeshProUGUI SliderValueText;
    [SerializeField] public int MaxBound;
    [SerializeField] public int MinBound;
    [SerializeField] public string PlayerPrefName;
}
[Serializable]
public class InputFieldSetting
{
    [SerializeField] public TMP_InputField InputFieldObject;
    [SerializeField] public int MaxBound;
    [SerializeField] public int MinBound;
    [SerializeField] public string PlayerPrefName;
}

public class OptionsMenuFunctionality : MonoBehaviour
{
    // Arrays to hold references to different setting objects (buttons, sliders and input fields)
    [SerializeField] private ButtonSetting[] buttonSettings;
    [SerializeField] private SliderSetting[] sliderSettings;
    [SerializeField] private InputFieldSetting[] inputFieldSettings;

    private void Start() // Called by unity when scene is loaded
    {
        loadSavedButtonValues(); // Calls LoadSavedButtonValues to highlight the buttons which are the saved player preferences (if there are any)
        loadSavedSliderValues(); // Calls LoadSavedSliderValues to change the slider/s + slider value/s next to it to the stored saved value (if there are any)
        loadSavedInputFieldValues(); // Calls LoadSavedInputFieldValues to load the input fields with saved values
    }


    // ----------------- PROCEDURES TO LOAD SAVED SETTING VALUES + DISPLAY THEM ON SCREEN: -----------------------

   // Procedure to set bold outlined button to the loaded saved button ref
    private void loadSavedButtonValues()
    {
        foreach (ButtonSetting buttonSetting in buttonSettings) // Goes through every button
        {
            // Changes current button to saved button (if no saved button, 0 is default):
            changeActiveButton(buttonSetting, PlayerPrefs.GetInt(buttonSetting.PlayerPrefName, 0));
        }
    }

    // Procedure to set the slider + slider value next to it to saved value
    private void loadSavedSliderValues()
    {
        Debug.Log("Loading saved slider values"); // Outputs that the start functions being executed for testing purposes

        foreach (SliderSetting sliderSetting in sliderSettings) // Goes through every slider
        {
            int newSliderValue = PlayerPrefs.GetInt(sliderSetting.PlayerPrefName, sliderSetting.MaxBound / 2); // Sets slider value to player saved value, if no value it sets it to default max value / 2
            sliderSetting.SliderObject.value = newSliderValue;
            sliderSetting.SliderValueText.text = newSliderValue.ToString();
        }
    }

    // Procedure to load input fields with their saved player value
    private void loadSavedInputFieldValues()
    {
        foreach (InputFieldSetting inputFieldSetting in inputFieldSettings)
        {
            inputFieldSetting.InputFieldObject.text = PlayerPrefs.GetInt(inputFieldSetting.PlayerPrefName, 1).ToString(); // Sets input fields text to saved value (1 as default if none saved)
        }
    }

    // ----------------------------------- CHANGE SETTING PROCEDURES -----------------------------------------

    // --- Change button procedures

    // Procedure to change the active setting (called via unities OnButtonClick for UI buttons)
    public void ChangeButton(int settingAndValueRef) // Has combined setting + value ref due to unity inspector limited to only 1 value passable through a method
    {
        int settingIndexRef = findFirstDigit(settingAndValueRef); // Finds buttonSetting array index through first digit of setting + value ref
        ButtonSetting buttonSetting = buttonSettings[settingIndexRef]; // Finds the referenced button setting 
        GameObject newButton = buttonSetting.ButtonGameObjects[findLastDigit(settingAndValueRef)]; // Finds the referenced button setting (by finding array index by getting last digit of ref)

        PlayerPrefs.SetInt(buttonSetting.PlayerPrefName, settingIndexRef); // Saves the chosen difficulty to player prefs (the difficulty variables passed via the onclick button function)

        Debug.Log($"Setting: {buttonSetting.PlayerPrefName} changed to: {newButton}."); // Log for debugging
    }

    // Procedure to change the active outlined button 
    private void changeActiveButton(ButtonSetting buttonSetting, int newButtonRef)
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

    // --- Change slider value procedure

    // Procedure to update the slider value saved to what's dispalyed (called by unity slider when updated)
    public void UpdateSliderValue(int sliderRef) // Method called when volume sliders value's changed (by unity slider)
    {
        SliderSetting sliderSetting = sliderSettings[sliderRef];

        // Updates saved playerpref value
        PlayerPrefs.SetInt(sliderSetting.PlayerPrefName, (int)sliderSetting.SliderObject.value);

    }

    public void UpdateInputFieldValue(int inputFieldRef)
    {
        InputFieldSetting inputFieldSetting = inputFieldSettings[inputFieldRef];
        TMP_InputField inputFieldObject = inputFieldSetting.InputFieldObject;

        if(int.TryParse(inputFieldObject.text, out int value) && value >= 0 && value <= 9) // Trys to parse from the string input to an int and set it to value + validates it's within bounds 0-9 (although bit redundant as input field has 1 char limit)
        {
            PlayerPrefs.SetInt(inputFieldSetting.PlayerPrefName, value);
            
        }
        else // If validation fails
        {
            inputFieldObject.text = PlayerPrefs.GetInt(inputFieldSetting.PlayerPrefName).ToString(); // Sets text back to saved value to show no value saved
        }


    }
    // Called when text's modified to show if the input's valid or not
    public void ValidateInputFieldValue(int inputFieldRef)
    {
        InputFieldSetting inputFieldSetting = inputFieldSettings[inputFieldRef];
        TMP_InputField inputFieldObject = inputFieldSetting.InputFieldObject;

        if (int.TryParse(inputFieldObject.text, out int value) && value >= 0 && value <= 9) // Trys to parse from the string input to an int and set it to value + validates it's within bounds 0-9 (although bit redundant as input field has 1 char limit)
        {
            inputFieldObject.textComponent.color = Color.green; // Changes text colour to green to show the input is valid
        }
        else // If validation fails
        {
            inputFieldObject.textComponent.color = Color.red; // Changes text colour to red to show the input is invalid
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
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gamescene"));
            InstanceReferences.Instance.GameSceneAudioListener.enabled = true;
            InstanceReferences.Instance.GameSceneEventSystemParent.SetActive(true);
            SceneManager.UnloadSceneAsync("OptionsMenu"); // If a game's active it only unloads the option menu because the users in the middle of the game
                                                          // and the scene is overlayed ontop of it.
        }
    }

} // ---------------------------------- END OF CLASS ------------------------------------------


