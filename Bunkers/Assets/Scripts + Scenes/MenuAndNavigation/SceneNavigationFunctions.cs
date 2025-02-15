using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationFunctions : MonoBehaviour
{
    private void Update() // Update is called once per frame
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // If escape key's pressed
        {
            GoToMainMenu(); // Calls GoToMainMenu procedure
        }

        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // unloads current scene and loads main menu scene
        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }

    public static void GoToStatisticsMenu()
    {
        SceneManager.LoadScene("StatisticsMenu"); // Unloads current scene and loads statistics menu
        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }

    public static void GoToOptionsMenu(bool additive)
    {
        if (additive) // If scene's loaded additively 
        {
            InstanceReferences.Instance.GameSceneAudioListener.enabled = false;
            InstanceReferences.Instance.GameSceneEventSystemParent.SetActive(false);
            SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive); // Loads the new scene additively
        }
        else
        {
            SceneManager.LoadScene("OptionsMenu"); // Otherwise it just loads it regularly
        }

        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }

    public static void GoToInformationMenu()
    {
        SceneManager.LoadScene("InformationMenu"); // Unloads current scene and loads information menu
        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }

    public static void GoToAIGame()
    {
        GeneralBackgroundLogic.StartGame(); // Calls start game procedure to start a game
        // MAINTANANCE - Could add audio feedback for menu navigatiopn change
    }
}
