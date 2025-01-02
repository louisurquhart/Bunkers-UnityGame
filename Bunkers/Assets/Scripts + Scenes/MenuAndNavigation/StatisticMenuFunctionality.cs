using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsMenuFunctionality : MonoBehaviour
{
    private const int _arrayCount = 2;
    private const int _competitiveArrayCount = 6;
    private const int _funArrayCount = 5;

    // Defines + Initializes two base arrays to hold player pref names + text 
    Array[] _playerPrefArrays = new Array[_arrayCount];
    Array[] _tmpTextArrays = new Array[_arrayCount];

    String[] _competitvePlayerPrefNames = new string[_competitiveArrayCount]
    {
        "TotalNumberOfHits",
        "TotalNumberOfMisses",
        "HitRate%",
        "Wins",
        "Losses",
        "WinRate%"
    };

    String[] _funPlayerPrefNames = new string[_funArrayCount]
    {
        "SpecialStrikesUsed",
        "TimeSpentInGame",
        "BunkersDestroyed",
        "GamesPlayed",
        "TotalGamesLaunched"
    };


    [SerializeField] TMP_Text[] _competitiveStatisticReferences = new TMP_Text[_competitiveArrayCount];
    [SerializeField] TMP_Text[] _funStatisticsReferences = new TMP_Text[_funArrayCount];

    private void Awake() // Method called immediately after class is initialized
    {
        // Adds the sub TMP arrays to the main TMP array
        _tmpTextArrays[0] = _competitiveStatisticReferences;
        _tmpTextArrays[1] = _funStatisticsReferences;

        // Adds the sub playerPrefName arrays to the name playerPrefName array array
        _playerPrefArrays[0] = _competitvePlayerPrefNames;
        _playerPrefArrays[1] = _funPlayerPrefNames;
    }


    private void Start()
    {
        loadStatisticValues(); // Saved statistics are loaded 
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
                Debug.Log($"Updating array: {i} at position: {k}");
                Debug.Log($"INFO: PlayerPrefValue: {PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0)} PlayerPrefName: {(string)currentPlayerPrefArray.GetValue(k)}");
                int loadedValue = PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0); // Finds the saved statistic value (if no saved value, default of 0's loaded)

                TMP_Text tmpText = (TMP_Text)currentGameObjectArray.GetValue(k); // Finds the TMP component in the array

                tmpText.text = loadedValue.ToString(); // Sets the TMP components text value to the saved statistic value
            }
        }
    }
}