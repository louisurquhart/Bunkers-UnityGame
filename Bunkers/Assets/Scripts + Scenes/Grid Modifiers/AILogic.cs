using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    private static GridManager gridManager = InstanceReferences.Instance.PlayerGridManager; // Creates a reference to the players grid manager

    static List<Tile> hitAliveBunkerTiles = new List<Tile>();
    static List<Tile> consecutiveHitAliveBunkerTiles = new List<Tile>();

    static Dictionary<int, Action> difficultyDictionary = new (3) // Dictionary to convert int difficulty values to their corrosponding methods (1 = Easy, 2 = Medium, 3 = Hard)
    {
        {0 , hardAI },
        {1 , mediumAI },
        {2 , easyAI }
    };

    // Initiate AI turn called when it's the AI's turn. Determines which AI method to call depending on difficulty
    public static void InitiateAITurn()
    {
        if (!CommonVariables.PlayerTurn) // Validates it isn't the players turn
        { 
            Debug.Log($"InitiateAITurn: Difficulty playerpref = {PlayerPrefs.GetInt("Difficulty", 0)}");
            int _difficultyKeyInt = PlayerPrefs.GetInt("Difficulty", 0); // Finds the saved difficulty key in PlayerPrefs
            difficultyDictionary[_difficultyKeyInt](); // Calls the method associated with the saved difficulty key
        }
        else // Otherwise if it is the players turn (validation failiure)
        {
            Debug.LogError("InitiateAITurn: AI turn called when it's not the AI's turn");
        }
    }

    // ------------------ EASY AI -------------------
    private static void easyAI() // Easy AI is very basic and only guesses random board positions
    {
        Debug.Log("EasyAI called");

        while (true) // Executes for as long as an unhit position hasn't been found
        {
            // ------------ Generates random tile: ---------------
            Tile randomTile = GeneralBackgroundLogic.GenerateRandomTile(gridManager); // Uses GenerateRandomTile function to find a random tile 

            // ----- Checks if random tile's previously hit: ----
            if (!randomTile.IsPreviouslyHit) // If the tile isn't previously hit
            {
                int tileHitSuccess = gridManager.OnTileHit(randomTile); // It calls OnTileHit

                // And then evaluates its success:
                if (isTileHitSuccess(tileHitSuccess) && randomTile.IsBunker)
                {
                    addAliveBunkerTile(randomTile); // If OnTileHit was successful it adds it to the alivebunkertile array
                }
                return;
            }
            else { continue; } // If tile has been hit before it reloops to try and find a tile which hasn't
        }
    }

    // ----------------- MEDIUM AI ------------------
    private static void mediumAI() // Medium AI is a moderately advanced AI which guesses around where previous bunkers have been hit but not fully destroyed until its destroyed the whole bunker.
    {
        Debug.Log("MediumAI called");

        // ---- Checks if there's any already hit alive bunkers to guess around ----
        if (hitAliveBunkerTiles.Count <= 0) // If there's no already hit alive bunkers it generates a random positon + checks if it was bunker
        {
            easyAI();
        }
        else // If there's one or more bunker tiles already hit it picks one and hits around it
        {
            Tile _randomNearbyBunkerTile = generateRandomNearbyBunkerTile();

            if (_randomNearbyBunkerTile == null)
            {
                mediumAI();
                return;
            }
            else if (_randomNearbyBunkerTile.IsBunker) // If the tile's a bunker
            {
                addAliveBunkerTile(_randomNearbyBunkerTile); // It adds it to the hitAliveBunkerTiles array for future turns to guess around this position
            }

            gridManager.OnTileHit(_randomNearbyBunkerTile); // Hits the tile generated
        }
    }

    // ------------------ HARD AI ------------------
    private static void hardAI() // Hard AI is a very advanced AI which guesses around where previous bunkers + countinues lines until a bunker has been fully destroyed.
    {
        mediumAI();
        return;

        // WIP:

        //Debug.Log("HardAI called");
        //if (hitAliveBunkerTiles.Count < 2) // Checks if there's multiple bunkers in a row to hit
        //{
        //    mediumAI();
        //    return;
        //}
        //else
        //{
        //}
    }

    // ----------------- BUNKER GENERATION SUB-PROCEDURES -------------------

    // ----------------- HIT AROUND BUNKER TILE SUB PROCEDURE -------------------

    // Procedure to generate a tile around a bunker which is alive and has already been hit

    private static Tile generateRandomNearbyBunkerTile()
    {
        // Finds a random tile which has already been hit + still has potential bunkers around it
        Tile _randomAnchorBunkerTile = hitAliveBunkerTiles[UnityEngine.Random.Range(0, hitAliveBunkerTiles.Count)]; // Finds a randomAliveBunker inside the hitAliveBunkerTiles array
        int _anchorTileRow = _randomAnchorBunkerTile.Row;
        int _anchorTileCol = _randomAnchorBunkerTile.Col;

        // Creates a list for storing the already checked relative positions (Col -1, Col + 1, Row -1, Row +1) so it can check if all surrounding positions have been checked
        List<int> numbersGenerated = new List<int>();

        // Generates a random tile which is around the anchor tile:
        
        while (true)
        {
            Tile _randomNearbyBunkerTile;

            // Generates a random guess relative to the tile (0 = Col-1, 1 =  Col+1, 2 = Row-1, 3 = Row+1)
            int _randomGuessInt = UnityEngine.Random.Range(0, 4);
            numbersGenerated.Add(_randomGuessInt);

            switch (_randomGuessInt) // Switch statement for random positions to check based on the random guess number generted
            {
                case 0:
                    if (_anchorTileCol == 0) { continue; }
                    _randomNearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol - 1]; // Checks col - 1 relative to the tile
                    break;
                case 1:
                    if (_anchorTileCol == 8) { continue; }
                    _randomNearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol + 1]; // Checks col + 1 relative to the tile
                    break;
                case 2:
                    if (_anchorTileRow == 0) { continue; }
                    _randomNearbyBunkerTile = gridManager.Grid[_anchorTileRow - 1, _anchorTileCol]; // Checks row - 1 relative to the tile
                    break;
                case 3:
                    if (_anchorTileRow == 8) { continue; }
                    _randomNearbyBunkerTile = gridManager.Grid[_anchorTileRow + 1, _anchorTileCol]; // Checks row + 1 relative to the tile
                    break;
                default: // If none of the cases are true (a logical code error)
                    Debug.LogError("MediumAI - Given input fit none of the cases"); // An error's output for fixing/testing
                    return null; // And the procedure is ended as there's an error with it
            }

            if (_randomNearbyBunkerTile.IsPreviouslyHit) // If tile has already been hit
            {
                // Loop exit condition if all posible positions have been hit (+ removal of hit bunker tile)
                if (numbersGenerated.Count >= 4) // If all possible guess positons for this alive bunker have been expended
                {
                    hitAliveBunkerTiles.Remove(_randomAnchorBunkerTile); // It removes the anchor tile from the array as it's no longer usable
                    return null;
                }
                continue; // Reloops to find another tile around it
            }

            else { return _randomNearbyBunkerTile; } // If the found tile hasn't been hit before the loop is broken
        }
    }

    private static bool isTileHitSuccess(int tileHitSuccess)
    {
        switch (tileHitSuccess) // Evaluates the success of OnTileHit (responds + returns errors if unsuccessful)
        {
            case 0: // If 0 is returned it means the OnTileHit function was carried out successfully

                return true; // Exits out the function returning the tile found

            case 1: // If 1 is returned it means the OnTileHit functions validation failed due to the tile already having been hit

                Debug.LogError("AI is trying to hit a tile which has already been hit"); // Outputs error
                return false; // Returns null as no unhit tile could be generated

            case 2: // If 2 is returned it means the OnTileHit functions validation failed due to the it not being the AI's turn

                Debug.LogError($"AI is trying to hit a tile when it's not their turn. (PlayerTurn = {CommonVariables.PlayerTurn})"); // Outputs error
                return false; // Exits out the function returning the tile found
        }

        // If tileHitSuccess fits none of the cases:
        Debug.LogError($"isTileHitSuccess: tileHitSuccess out of bounds (tileHitSuccess == {tileHitSuccess}"); // Outputs error
        return false; // Returns false to signify it was unsucessful
    }
    


    private static void addAliveBunkerTile(Tile hitAliveBunkerTile)
    {
        // --- Adds it to the hitaliveBunkerTile array
        hitAliveBunkerTiles.Add(hitAliveBunkerTile);

        // --- Evaluates if it's a string of bunkers or just on its own. If it is it adds it to the aliveBunkerTile array ---
        Tile nearbyBunkerTile;
        bool vertical;
        int _anchorTileRow = hitAliveBunkerTile.Row;
        int _anchorTileCol = hitAliveBunkerTile.Col;

        // Checks left + right columns (horizonal)
        if (_anchorTileCol != 0 && hitAliveBunkerTiles.Contains(gridManager.Grid[_anchorTileRow, _anchorTileCol - 1]))
        {
            nearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol - 1];
            vertical = false;
        }
        else if (_anchorTileCol != 8 && hitAliveBunkerTiles.Contains(gridManager.Grid[_anchorTileRow, _anchorTileCol + 1]))
        {
            nearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol + 1];
            vertical = false;
        }
        // Checks up + down rows (vertical)
        else if (_anchorTileCol != 0 && hitAliveBunkerTiles.Contains(gridManager.Grid[_anchorTileRow, _anchorTileCol - 1]))
        {
            nearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol - 1];
            vertical = true;
        }
        else if (_anchorTileRow != 8 && hitAliveBunkerTiles.Contains(gridManager.Grid[_anchorTileRow, _anchorTileCol + 1]))
        {
            nearbyBunkerTile = gridManager.Grid[_anchorTileRow, _anchorTileCol + 1];
            vertical = true;
        }
        else { return; } // Otherwise if there's no nearby bunker tiles 



    }

}



