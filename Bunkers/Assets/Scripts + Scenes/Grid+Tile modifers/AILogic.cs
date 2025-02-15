using System;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    private static GridManager _gridManager = InstanceReferences.Instance.PlayerGridManager; // Creates a reference to the players grid manager

    // Array to store the tiles which are part of a bunker which has been hit but not fully destroyed
    static List<Tile> _hitAliveBunkerTiles = new List<Tile>();

    static Dictionary<int, Action> _difficultyDictionary = new (3) // Dictionary to convert int difficulty values to their corrosponding methods (1 = Easy, 2 = Medium, 3 = Hard)
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
            _difficultyDictionary[_difficultyKeyInt](); // Calls the method associated with the saved difficulty key
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
            Tile randomTile = GeneralBackgroundLogic.GenerateRandomTile(_gridManager); // Uses GenerateRandomTile function to find a random tile 


            // ----- Checks if random tile's previously hit: ----
            if (!randomTile.IsPreviouslyHit) // If the tile isn't previously hit
            {
                hitPlayerTile(randomTile); // Calls the hitPlayerTile procedure to hit the tile + with evaluation and potential special strike use

                // And then evaluates its success:
                if (randomTile.IsBunker) // If the tile's a bunker
                {
                    _hitAliveBunkerTiles.Add(randomTile); // It adds it to the hitAliveBunkerTiles array for future turns to guess around this position
                    Debug.Log("Bunker tile added to array");
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
        if (_hitAliveBunkerTiles.Count <= 0) // If there's no already hit alive bunkers it generates a random positon + checks if it was bunker
        {
            easyAI();
        }
        else // If there's one or more bunker tiles already hit it picks one and hits around it
        {
            Tile _randomNearbyBunkerTile = generateRandomNearbyBunkerTile();

            if (_randomNearbyBunkerTile == null) // If null is returned it means all posible tiles around the random anchor bunker tile have been hit.
                                                 // Medium AI calls itself recursively to recheck there's bunkers to hit around and if so try again
            {
                mediumAI();
                return;
            }
            else if (_randomNearbyBunkerTile.IsBunker) // If the tile's a bunker
            {
                _hitAliveBunkerTiles.Add(_randomNearbyBunkerTile); // It adds it to the hitAliveBunkerTiles array for future turns to guess around this position
                Debug.Log("Bunker tile added to array");
            }

            hitPlayerTile(_randomNearbyBunkerTile); // Calls the hitPlayerTile procedure to hit the tile + with evaluation and potential special strike use
        }
    }

    // ------------------ HARD AI ------------------
    private static void hardAI() // Hard AI is a very advanced AI which guesses around where previous bunkers + countinues lines until a bunker has been fully destroyed.
    {
        // Not implemented so calls mediumAI for the time being
        mediumAI();

        // MAINTANANCE - Could add hard AI algorithm here

        return;
    }
    // ----------------- BUNKER GENERATION SUB-PROCEDURES -------------------
    private static void hitPlayerTile(Tile givenTile) // Procedure to hit a player tile + with the option of a special strike
    {
        Debug.Log("HitPlayerTile called");
        if (PlayerPrefs.GetInt("SpecialStrikeStatus") == 0) // If special strikes are enabled (SpecailStrikeStatus == 0)
        {
            Debug.Log("HitPlayerTile - identified special strikes are enabled");
            SpecialStrikeFunctionality specialStrikeFunctionality = _gridManager.SpecialStrikeFunctionality;

            foreach(SpecialStrikeWeapon specialStrikeWeapon in specialStrikeFunctionality.Weapons) // Goes through each specail strike weapon in weapons array
            {
                // Generates a random special strike weapon and assigns its reference to the variable randomWeapon 
                int randomWeaponIndexNum = UnityEngine.Random.Range(0, specialStrikeFunctionality.Weapons.Length);
                SpecialStrikeWeapon randomWeapon = specialStrikeFunctionality.Weapons[randomWeaponIndexNum];

                // Checks that the weapon has uses left
                if (randomWeapon.TotalUsesLeft > 0)
                {
                    // Activates random weapon and exits the loop
                    randomWeapon.Activate(givenTile.Row, givenTile.Col, _gridManager);
                    break;
                }
                else
                {
                    continue; // Otherwise for loop's continued to find a weapon with uses left 
                }
            }
        }
        else // Otherwise if no special strikes enabled, the tile's hit normally
        {
            Debug.Log("HitPlayerTile - special strikes aren't enabled");
            _gridManager.OnTileHit(givenTile, true);
        }
    }
    
    // ----------------- HIT AROUND BUNKER TILE SUB PROCEDURE -------------------

    // Procedure to generate a tile around a bunker which is alive and has already been hit
    private static Tile generateRandomNearbyBunkerTile()
    {
        // Finds a random tile which has already been hit + still has potential bunkers around it
        Tile _randomAnchorBunkerTile = _hitAliveBunkerTiles[UnityEngine.Random.Range(0, _hitAliveBunkerTiles.Count)]; // Finds a randomAliveBunker inside the hitAliveBunkerTiles array
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
                    _randomNearbyBunkerTile = _gridManager.Grid[_anchorTileRow, _anchorTileCol - 1]; // Checks col - 1 relative to the tile
                    break;
                case 1:
                    if (_anchorTileCol == 8) { continue; }
                    _randomNearbyBunkerTile = _gridManager.Grid[_anchorTileRow, _anchorTileCol + 1]; // Checks col + 1 relative to the tile
                    break;
                case 2:
                    if (_anchorTileRow == 0) { continue; }
                    _randomNearbyBunkerTile = _gridManager.Grid[_anchorTileRow - 1, _anchorTileCol]; // Checks row - 1 relative to the tile
                    break;
                case 3:
                    if (_anchorTileRow == 8) { continue; }
                    _randomNearbyBunkerTile = _gridManager.Grid[_anchorTileRow + 1, _anchorTileCol]; // Checks row + 1 relative to the tile
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
                    _hitAliveBunkerTiles.Remove(_randomAnchorBunkerTile); // It removes the anchor tile from the array as it's no longer usable
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

    // MAINTANANCE - Could add function to check if a bunker added is next to another bunker here (for future HardAI() implementation)

}



