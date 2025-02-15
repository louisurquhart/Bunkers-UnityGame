using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatisticsMenuFunctionality : MonoBehaviour
{
    // MAINTANANCE - could chane this to object oriented apporach with each statistic being an object

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
        "MinutesSpentInGame",
        "FullBunkersDestroyed",
        "MidgameQuits",
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

        // MAINTANANCE - Could add audio feedback for all statistics loaded
    }

    // --------- Procedure to calculate values for statistics dependent on other statistics --------------
    private void calculateDependentStatisticValues()
    {
        // ---- Calculate hitrate% ----

        // Gets values of hits + misses from playerprefs for calculation
        float hits = PlayerPrefs.GetInt("TotalNumberOfHits");
        float misses = PlayerPrefs.GetInt("TotalNumberOfMisses");

        if (hits != 0 && misses != 0)
        {
            // Calculates + sets hit rate using these values
            float totalStrikes = hits + misses;
            float hitRate = hits / totalStrikes;
            float hitRatePercent = hitRate * 100;
            PlayerPrefs.SetInt("HitRate%", (int)hitRatePercent);
        }


        // ---- Calculating winrate% ----

        // Gets values of wins + losses from playerprefs for calculation
        float wins = PlayerPrefs.GetInt("Wins");
        float losses = PlayerPrefs.GetInt("Losses");

        if (wins != 0 && losses != 0)
        {
            // Calculates + sets win rate using these values
            float totalGames = wins + losses; // Calculates total games played (wins + losses)
            float winRate = wins / totalGames; // Divides wins / total games played to get winrate % (as decimal 0-1
            float winRatePercent = winRate * 100;
            PlayerPrefs.SetInt("WinRate%", (int)winRatePercent); // Sets WinRate% playerpref to the winrate value (* 100 to make it a %)
        }


        // ---- Calculate minutes spent in game (by default they're seconds) -----
        PlayerPrefs.SetInt("MinutesSpentInGame", PlayerPrefs.GetInt("MinutesSpentInGame", 0) / 60); // Sets minutes spent in game to minutes spent in game /60 (as by default they're seconds, need converting to minutes)
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
                //Debug.Log($"Loading values from array: {i} at position: {k}");
                //Debug.Log($"loadStatisticsValues: Loading PlayerPrefName: {(string)currentPlayerPrefArray.GetValue(k)} with value: PlayerPrefValue: {PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0)} ");
                int loadedValue = PlayerPrefs.GetInt((string)currentPlayerPrefArray.GetValue(k), 0); // Finds the saved statistic value (if no saved value, default of 0's loaded)

                TMP_Text tmpText = (TMP_Text)currentGameObjectArray.GetValue(k); // Finds the TMP component in the array

                tmpText.text = loadedValue.ToString(); // Sets the TMP components text value to the saved statistic value
            }
        }
    }

    public void ResetStatisticValues()
    {
        for (int i = 0; i < _arrayCount; i++) // Loops through the sub arrays in the main array
        {
            Array currentPlayerPrefArray = _playerPrefArrays[i]; // Creates a reference to the current PlayerPrefName array (for maintainability + easy referencing)
            Array currentGameObjectArray = _tmpTextArrays[i]; // Creates a reference to the current GameObject array (for maintainability + easy referencing)

            for (int k = 0; k < currentGameObjectArray.Length; k++) // Loops through for each value in the array to reset each statistic value
            {
                // Log for testing:
                //Debug.Log($"ResetStatisticsValues: Resetting PlayerPref: {(string)currentPlayerPrefArray.GetValue(k)}");

                // Sets the playerpref value at the current key to 0
                PlayerPrefs.SetInt((string)currentPlayerPrefArray.GetValue(k), 0); // Sets the saved statistic value to 0
            }
        }

        loadStatisticValues(); // After all statistics are reset, it reloads the values displayed on screen with them
    }


    // Public static method to update a statistic value - to be called when an event in game occurs requiring statistic update
    public static void IncrementStatisticValue(string statisticKey)
    {
        int statisticRecordingStatus = PlayerPrefs.GetInt("StatisticRecordingStatus", 0);
        int statisticRecordingType = PlayerPrefs.GetInt("StatisticRecordingType", 0);

        if (statisticRecordingStatus == 0) // if statistics recording status == 0 it's enabled so update code runs
        {
            if (_competitivePlayerPrefNames.Contains(statisticKey) && statisticRecordingStatus == 0 || statisticRecordingStatus == 1) // If the statisticKey belongs to competitive statistics && competitive statistic recording's enabled (ALL || COMPETITIVE)
            {
                // It increments the playerPref value which corrosponds to the given key
                PlayerPrefs.SetInt(statisticKey, PlayerPrefs.GetInt(statisticKey) + 1);

            }
            else if (_funPlayerPrefNames.Contains(statisticKey) && statisticRecordingStatus == 0 || statisticRecordingStatus == 2) // If the statisticKey belongs to fun statistics && fun statistic recording's enabled (ALL || FUN)
            {
                // It increments the playerPref value which corrosponds to the given key
                PlayerPrefs.SetInt(statisticKey, PlayerPrefs.GetInt(statisticKey) + 1);
            }
            else
            {
                Debug.Log("IncrementStatisticValue called but no action performed due to invalid key/specific key category recording not enabled");
                return;
            }

            Debug.Log($"Statistic: {statisticKey} incremented by 1");
        }
        else
        {
            Debug.Log($"IncrementStatisticValue called but no action performed due recording disabled (StatisticRecordingStatus == {PlayerPrefs.GetInt("StatisticRecordingStatus", 0)} (0 = enabled, 1 = disabled)");
        }
    }
}