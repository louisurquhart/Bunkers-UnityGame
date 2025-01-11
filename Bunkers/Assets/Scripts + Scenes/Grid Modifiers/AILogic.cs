using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    private static GridManager gridManager = InstanceReferences.Instance.PlayerGridManager; // Creates a reference to the players grid manager

    static Dictionary<int, Action> difficultyDictionary = new Dictionary<int, Action>(3) // Dictionary to convert int difficulty values to their corrosponding methods (1 = Easy, 2 = Medium, 3 = Hard)
    {
        {1 , easyAI },
        {2 , mediumAI },
        {3 , hardAI }
    };

    // Initiate AI turn called when it's the AI's turn. Determines which AI method to call depending on difficulty
    public static void InitiateAITurn()
    { 
        int difficultyKeyInt = PlayerPrefs.GetInt("Difficulty", 0); // Finds the saved difficulty key in PlayerPrefs
        difficultyDictionary[difficultyKeyInt](); // Calls the method associated with the saved difficulty key
    }


    // ------------------ EASY AI ------------------
    private static void easyAI() // Easy AI is very basic and only guesses random board positions
    {
        // Test code
        Debug.Log($"Executing EasyAI turn");

        // Validates it's the AI's turn
        if (CommonVariables.PlayerTurn)
        {
            Debug.LogError("AILogic - EasyAI: AITurn has been called when it's not the AI's turn");
            return;
        }

        bool noPositionGenerated = true;
        int iterations = 0;

        while (noPositionGenerated && iterations <= 1000) // Executes for as long as an unhit position hasn't been found + iterations haven't exceeded 1000 (infinite loop prevention whilist testing)
                                                          // (Virtually impossible to not have generated a random position after this many iterations)
        {
            iterations++; // Increments iterations

            Tile randomTile = generateRandomTile(gridManager); // Uses GenerateRandomTile function to find a random tile 

            int tileHitSuccess;

            if (!(randomTile.IsPreviouslyHit)) // Checks that the randomTile isn't previously hit + that the function hasn't failed and iterated more than 10 times
            {
                tileHitSuccess = gridManager.OnTileHit(randomTile); // Calls the OnTileHit for the randomly generated tile. This also returns a value to indicate success which is evaluated immediately after

                switch (tileHitSuccess) // Evaluates the success of OnTileHit (responds + returns errors if unsuccessful)
                {
                    case 0: // If 0 is returned it means the OnTileHit function was carried out successfully

                        return; // Exits out the function

                    case 1: // If 1 is returned it means the OnTileHit functions validation failed due to the tile already having been hit

                        Debug.LogError("AI is trying to hit a tile which has already been hit"); // Outputs error
                        continue; // Takes remedial action continuing the function to try find a tile which hasn't been hit. (infinite loop is prevented by iteration limit)

                    case 2: // If 2 is returned it means the OnTileHit functions validation failed due to the it not being the AI's turn

                        Debug.LogError($"AI is trying to hit a tile when it's not their turn. (PlayerTurn = {CommonVariables.PlayerTurn})"); // Outputs error
                        return; // Exits out the function
                }
            }
            else
            {
                continue; // If tile has been hit before it reloops to try and find a tile which hasn't
            }
        }
    }

    // Generate random tile method made from reactoring of EasyAI (returns a random tile)
    private static Tile generateRandomTile(GridManager gridManager) 
    {
        // Generates a random row + column (position)
        int randomRow = UnityEngine.Random.Range(0, 9); // Generates a random row
        int randomColumn = UnityEngine.Random.Range(0, 9); // Generates a random column

        // Finds the tile located at the randomly generated position through the grid managers Grid array which stores all the tiles 
        Tile randomTile = gridManager.Grid[randomRow, randomColumn];

        return randomTile; // Returns the random tile
    }




    // ------------------ MEDIUM AI ------------------
    private static void mediumAI() // Medium AI is a moderately advanced AI which guesses around where previous bunkers have been hit but not fully destroyed until its destroyed the whole bunker.
    {
        easyAI(); // Medium difficulty will be added in final iteration. Calls EasyAI in the meantime
    }



    // ------------------ HARD AI ------------------
    private static void hardAI() // Hard AI is a very advanced AI which guesses around where previous bunkers + countinues lines until a bunker has been fully destroyed.
    {
        easyAI(); // Hard difficulty will be added in final iteration. Calls EasyAI in the meantime
    }

}
