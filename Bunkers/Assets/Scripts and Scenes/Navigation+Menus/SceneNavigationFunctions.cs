using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationFunctions : MonoBehaviour
{
    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // unloads current scene and loads main menu scene
    }

    public void GoToStatisticsMenu()
    {
        SceneManager.LoadScene("StatisticsMenu"); // unloads current scene and loads statistics menu
    }

    public static void GoToOptionsMenu(bool additive)
    {
        if (additive)
        {
            SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("OptionsMenu");
        }
    }

    public static void GoToInformationMenu()
    {
        SceneManager.LoadScene("InformationMenu");
    }

    public static void GoToAIGame()
    {
        GeneralBackgroundLogic.StartGame();
    }

    
}
