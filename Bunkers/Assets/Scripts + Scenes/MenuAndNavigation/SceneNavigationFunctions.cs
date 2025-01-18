using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneNavigationFunctions : MonoBehaviour
{
    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // unloads current scene and loads main menu scene
    }

    public static void GoToStatisticsMenu()
    {
        SceneManager.LoadScene("StatisticsMenu"); // Unloads current scene and loads statistics menu
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
    }

    public static void GoToInformationMenu()
    {
        SceneManager.LoadScene("InformationMenu"); // Unloads current scene and loads information menu
    }

    public static void GoToAIGame()
    {
        GeneralBackgroundLogic.StartGame(); // Calls start game procedure to start a game
    }
}
