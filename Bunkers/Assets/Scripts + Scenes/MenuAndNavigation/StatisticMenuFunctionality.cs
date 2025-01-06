using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatisticsMenuFunctionality : MonoBehaviour
{
    // Const values set for array lengths (used for initializing arrays + looping through arrays)
    private const int _arrayCount = 2;
    private const int _competitiveArrayCount = 6;
    private const int _funArrayCount = 5;

    // 2 arrays initialized to hold the playerPrefArrays + TMP reference a
    Array[] _playerPrefArrays = new Array[_arrayCount];
    Array[] _tmpTextArrays = new Array[_arrayCount];

    static String[] _competitivePlayerPrefNames = new string[_competitiveArrayCount] // Array to store the keys for the competitive playerpref statistics
    {
        "TotalNumberOfHits",
        "TotalNumberOfMisses",
        "HitRate%",
        "Wins",
        "Losses",
        "WinRate%"
    };

    static String[] _funPlayerPrefNames = new string[_funArrayCount] // Array to store the keys for the fun playerpref statistics
    {
        "SpecialStrikesUsed",
        "TimeSpentInGame",
        "BunkersDestroyed",
        "GamesPlayed",
        "TotalGamesLaunched"
    };


    // Array to store the TMP component references for the competitive + fun statistics (actual references set in unity inspector)
    [SerializeField] TMP_Text[] _competitiveStatisticReferences = new TMP_Text[_competitiveArrayCount];
    [SerializeField] TMP_Text[] _funStatisticsReferences = new TMP_Text[_funArrayCount];


    private void Awake() // Method called immediately after class is created
    {
        // Adds the sub TMP arrays to the main TMP array
        _tmpTextArrays[0] = _competitiveStatisticReferences;
        _tmpTextArrays[1] = _funStatisticsReferences;

        // Adds the sub playerPrefName arrays to the name playerPrefName array array
        _playerPrefArrays[0] = _competitivePlayerPrefNames;
        _playerPrefArrays[1] = _funPlayerPrefNames;
    }


    private void Start() // Method called on scene load
    {
        calculateDependentStatisticValues(); // Calculates statistic values which are dependent on one other values
        loadStatisticValues(); // Saved statistics are loaded onto the screen
    }

    // --------- Procedure to calculate values for statistics dependent on other statistics --------------
    private void calculateDependentStatisticValues()
    {
        // ---- Calculate hitrate% ----

        // Gets values of hits + misses from playerprefs for calculation
        int hits = PlayerPrefs.GetInt("TotalNumberOfHits");
        int misses = PlayerPrefs.GetInt("TotalNumberOfMisses");

        // Calculates  + sets hit rate using these values
        int totalShots = hits + misses; // Calculates total games played (wins + losses)
        int hitRate = hits / totalShots; // Divides wins / total games played to get winrate % (as decimal 0-1)
        PlayerPrefs.SetInt("WinRate%", totalShots * 100); // Sets WinRate% playerpref to the winrate value (* 100 to make it a %)

        // ---- Calculating winrate% ----

        // Gets values of wins + losses from playerprefs for calculation
        int wins = PlayerPrefs.GetInt("Wins");
        int losses = PlayerPrefs.GetInt("Losses");

        // Calculates + sets win rate using these values
        int totalGames = wins + losses; // Calculates total games played (wins + losses)
        int winRate = wins / totalGames; // Divides wins / total games played to get winrate % (as decimal 0-1)
        PlayerPrefs.SetInt("WinRate%", winRate * 100); // Sets WinRate% playerpref to the winrate value (* 100 to make it a %)
    }


    // ------------ Procedure to load the players statistics onto the menu's text values --------------
    private void loadStatisticValues()
    {
        for (int i = 0; i < _arrayCount; i++) // Loops through the sub arrays in the main array
        {
            Array currentPlayerPrefArray = _playerPrefArrays[i]; // Creates a reference to the current PlayerPrefName array (for maintainability + easy referencing)
            Array currentGameObjectArray = _tmpTextArrays[i]; // Creates a reference to the current GameObject array (for maintainability + easy referencing)

            for (int k = 0; k < currentGameObjectArray.Length; k++) // Loops through for each value in the array to load each statistic value for each in game text value
            {
                Debug.Log($"Loading values from array: {i} at position: {k}");
                Debug.Log($"loadStatisticsValues: Loading PlayerPrefName: {(string)currentPlayerPrefArray.GetValue(k)} with value: PlayerPrefValue: {PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0)} ");
                int loadedValue = PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0); // Finds the saved statistic value (if no saved value, default of 0's loaded)

                TMP_Text tmpText = (TMP_Text)currentGameObjectArray.GetValue(k); // Finds the TMP component in the array

                tmpText.text = loadedValue.ToString(); // Sets the TMP components text value to the saved statistic value
            }
        }
    }

    // Public static method to update a statistic value - to be called when an event in game occurs requiring statistic update
    public static void IncrementStatisticValue(string statisticKey)
    {
        int statisticRecordingStatus = PlayerPrefs.GetInt("StatisticRecordingStatus", 1);
        int statisticRecordingType = PlayerPrefs.GetInt("StatisticRecordingType", 1);

        if (statisticRecordingStatus == 1) // if statistics recording status == 1 it's enabled so update code runs
        {
            if (_competitivePlayerPrefNames.Contains(statisticKey) && statisticRecordingStatus == 1 || statisticRecordingStatus == 2) // If the statisticKey belongs to competitive statistics && competitive statistic recording's enabled
            {
                // It increments the playerPref value which corrosponds to the given key
                PlayerPrefs.SetInt(statisticKey, PlayerPrefs.GetInt(statisticKey) + 1);

            }
            else if (_funPlayerPrefNames.Contains(statisticKey) && statisticRecordingStatus == 1 || statisticRecordingStatus == 3) // If the statisticKey belongs to fun statistics && fun statistic recording's enabled
            {
                // It increments the playerPref value which corrosponds to the given key
                PlayerPrefs.SetInt(statisticKey, PlayerPrefs.GetInt(statisticKey) + 1);
            }
        }
    }
}