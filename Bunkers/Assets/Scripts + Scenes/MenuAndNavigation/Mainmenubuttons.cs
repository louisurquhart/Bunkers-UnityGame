using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctionality : MonoBehaviour
{
    public static void SingleplayerButtonScript()
    {
        GeneralBackgroundLogic.StartGame(); // calls the start a game function (inputed int value 0 to show singleplayer)
    }
    public static void MultiplayerButtonScript()
    {
        GeneralBackgroundLogic.StartGame(); // calls the start a game function (inputed int value 1 to show multiplayer)
    }
    public static void StatisticsButtonScript()
    {
        SceneNavigationFunctions.GoToStatisticsMenu(); // calls go to statistics menu function
    }
    public static void OptionsButtonScript()
    {
        SceneNavigationFunctions.GoToOptionsMenu(true); // call go to options menu function (parameter false to show it's not additive)
    }
}
