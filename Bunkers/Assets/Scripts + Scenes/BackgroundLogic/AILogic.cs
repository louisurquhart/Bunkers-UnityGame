using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    List<int> previouslyHitBunkers = new List<int>();

    static GridManager gridManager = InstanceReferences.Instance.PlayerGridManager;

    static Dictionary<int, Action> difficultyDictionary = new Dictionary<int, Action>(3) // Dictionary to convert int difficulty values to their methods
    {
        {1 , EasyAI },
        {2 , MediumAI },
        {3 , HardAI }
    };

    public static void InitiateAITurn()
    { 
        int difficultyKeyInt = PlayerPrefs.GetInt("Difficulty", 0); // Finds the saved difficulty key in PlayerPrefs
        Task.Delay(1500); // Delays thread execution for 1.5s to add suspense
        difficultyDictionary[difficultyKeyInt](); // Calls the method associated with the saved difficulty key
    }

    public static void EasyAI() // Easy AI is very basic and only guesses random board positions
    {
        Debug.Log($"{CommonVariables.DebugFormat[0]}Executing EasyAI turn");

        // Validates it's the AI's turn
        if (CommonVariables.PlayerTurn)
        {
            Debug.LogError("AILogic - EasyAI: AITurn has been called when it's not the AI's turn");
            return;
        }

        bool noPositionGenerated = true;
        int iterations = 0;

        while (noPositionGenerated && iterations <= 100)
        {
            iterations++;

            Tile randomTile = GenerateRandomTile(gridManager); // Uses GenerateRandomTile function to find a random tile 

            int tileHitSuccess;

            if (!(randomTile.IsPreviouslyHit)) // Checks that the randomTile isn't previously hit + that the function hasn't failed and iterated more than 10 times
            {
                tileHitSuccess = gridManager.OnTileHit(randomTile);

                switch (tileHitSuccess) // Initiates the OnTileHit function for the random tile to tell it has been hit. It also evaluates if the bool return value wasn't successful for remedial action 
                {
                    case 0: // If 0 is returned it means the OnTileHit function was carried out successfully
                        return; // Exits out the function

                    case 1: // If 1 is returned it means the OnTileHit functions validation failed due to the tile already having been hit
                        Debug.LogError("AI is trying to hit a tile which has already been hit");
                        continue; // Takes remedial action continuing the function to try find a tile which hasn't been hit. (infinite loop is prevented by iteration limit)

                    case 2: // If 2 is returned it means the OnTileHit functions validation failed due to the it not being the AI's turn
                        Debug.LogError($"AI is trying to hit a tile when it's not their turn. (PlayerTurn = {CommonVariables.PlayerTurn})");
                        return; // Exits out the function
                }
            }
            else
            {
                continue;
            }
        }
    }

    public static Tile GenerateRandomTile(GridManager gridManager) // Method to return a random tile
    {
        int randomRow = UnityEngine.Random.Range(0, 9); // Generates a random row
        int randomColumn = UnityEngine.Random.Range(0, 9); // Generates a random column
        //Debug.Log($"Random position generated = Row: {randomRow} Column: {randomColumn}");
        Tile randomTile = gridManager.Grid[randomRow, randomColumn]; // Finds the position of the random row + column combined and the tile located at it (through the referenced gridmanager)

        return randomTile; // Returns the random tile
    }

    public static void MediumAI() // Medium AI is a moderately advanced AI which guesses around where previous bunkers have been hit but not fully destroyed until its destroyed the whole bunker.
    {
        EasyAI(); // Medium difficulty will be added in final iteration
    }

    public static void HardAI() // Hard AI is a very advanced AI which guesses around where previous bunkers + countinues lines until a bunker has been fully destroyed.
    {
        EasyAI(); // Hard difficulty will be added in final iteration
    }





}
