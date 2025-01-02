using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI.Table;

public class BunkerGenerator : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] int entity;

    public FullBunker[] Bunkers = new FullBunker[5]; // makes an array storing different types of bunkers

    private void Awake()
    {
        Bunkers[0] = new FullBunker(2, 2, Color.green, 1, gridManager); // 0th position in array
        Bunkers[1] = new FullBunker(4, 1, Color.red, 2, gridManager); // 1st position in array
        Bunkers[2] = new FullBunker(3, 1, Color.magenta, 3, gridManager); // 2nd position in array
        Bunkers[3] = new FullBunker(2, 1, Color.gray, 4, gridManager); // 3rd position in array
        Bunkers[4] = new FullBunker(5, 1, Color.black, 5, gridManager); // 4th position in array
    }


    public void RandomBunkerGenerator() // random bunker generator. Will be used for the AI's grid + potentially the players if selected in options. Input 0 = Player, Input 1 = AI
    {
        for (int i = 0; i < Bunkers.Length; i++) // Executes loop for the length of the bunker array (creates 1 of each bunkers)
        {
            FullBunker bunkerType = Bunkers[i]; // Finds the bunker which will be added to the board in the loop iteration

            randomlyRotateBunker(bunkerType); // Generates a random rotation for the bunker

            (int randomRow, int randomColumn, bool positionValidated) =  generateRandomValidatedBunkerPosition(bunkerType, gridManager); // Generates a random position for the bunker

            if (positionValidated) // If the bunker randomly generated position has been validated
            {
                placeFullBunker(randomRow, randomColumn, gridManager, bunkerType); // Calls placeBunker to assign all the grid squares which the bunker takes up as bunkers
            }
            else
            {
                Debug.LogError($"{CommonVariables.DebugFormat[entity]}Failed to generate a random bunker position");
            }
        }

        Debug.Log($"{CommonVariables.DebugFormat[entity]}Final bunker count - PlayerBunkerCount: {CommonVariables.BunkerCountsDictionary[0].Get()}, AIBunkerCount: {CommonVariables.BunkerCountsDictionary[1].Get()}. (Should be {Bunkers.Length} each)");
        Debug.Log($"{CommonVariables.DebugFormat[entity]}Variables set to - PlayerBunkerCount: {CommonVariables.PlayerAliveBunkerCount}, AIBunkerCount: {CommonVariables.AIAliveBunkerCount}");

    }

    private static (int, int, bool) generateRandomValidatedBunkerPosition(FullBunker bunker, GridManager gridManager)
    {
        int iterations = 0;

        while (iterations < 10000) // Loop repeats until a position has been found or if it has repeated 10,000 times which means that there's an issue with the code                                         // (iteration check is done for testing to prevent infinite loop + softlock)
        {
            // Generates random bunker position
            int randomRow = UnityEngine.Random.Range(0, 10 - bunker.Rows); // Generates a random row number
            int randomColumn = UnityEngine.Random.Range(0, 10 - bunker.Columns); // Generates a random column number

            if (!doesBunkerOverlap(randomRow, randomColumn, bunker.Rows, bunker.Columns, gridManager)) // V                                                                                                                      // alidates that the bunker won't overlap with any other bunkers
            {
                //Debug.Log("Bunker doesn't overlap so generating a bunker");
                return (randomRow, randomColumn, true); // sets external variable randomrow to the randomly generated row
            }
            iterations++;
        }

        return (0, 0, false); // If no position could be generated in 10000 iterations 0, 0 + false is returned to signify failiure
    }

    private static bool doesBunkerOverlap(int row, int column, int bunkerRows, int bunkerColumns, GridManager gridManager)
    {
        //Debug.Log("Checking if bunker overlaps");
        for (int r = row; r < row + bunkerRows; r++)
        {
            for (int c = column; c < column + bunkerColumns; c++)
            {
                if (gridManager.Grid[r, c].IsBunker) // If the position is a bunker it means it would be overlapping another bunker if placed, loop restarts as another position has to be found
                {
                    //Debug.Log($"INFO: Overlap found at position: Row = {row} Column = {column}");
                    return true; // If a bunker is found in the way, true is returned as the would be position overlaps.
                }
            }
        }
        return false; // If the for loops go through all the bunker positons and finds no bunkers in the way, false is returned to signify there's no overlap
    }

    private static void randomlyRotateBunker(FullBunker bunker)
    {
        int randomRotation = UnityEngine.Random.Range(0, 2); // Generates a random rotation for the bunker. 0 = horizontal, 1 = vertical

        if (randomRotation == 0) // If the rotation is horizontal (0) it switches the rows + columns around. By default they're vertical so no changing needs to be done then
        {
            int temprows = bunker.Rows; // sets a temporary variable to == rows so rows can be set to columns and the value isn't lost
            bunker.Rows = bunker.Columns; // rows are set equal to columns
            bunker.Columns = temprows; // columns are set equal to rows
        }
    }

    private static void placeFullBunker(int row, int column, GridManager gridManager, FullBunker bunkerType)
    {
        // -- Calculates the total amount of rows and columns the bunker will take up --
        int finalRow = row + bunkerType.Rows;
        int finalColumn = column + bunkerType.Columns;

        // -- Initiates the bunkers tile array so tiles can be added in the for loop --
        bunkerType.bunkerTiles = new Tile[finalRow * finalColumn];

        // -- Code for testing --
        int iterations = 1;
        int totalBunkersPlaced = bunkerType.Rows * bunkerType.Columns; // TotalBunkersPlaced calculated for debug log (testing)
        Debug.Log($"<b>{CommonVariables.DebugFormat[gridManager.EntityNum]}Placing full bunker (Number: {bunkerType.NumberIdentifier}). Total bunkers placed should be {totalBunkersPlaced}"); // Debug log output for testing

        // -- 2 for loops to go through every tile the bunker occupies and sets them to bunker tiles --
        for (int r = row; r < finalRow ; r++) // Goes over every row the bunker needs to occupy
        {
            for (int c = column; c <  finalColumn ; c++) // For every row it gom es through every column on that row that the bunker needs to occupy.
            {
                // Code for testing
                Debug.Log($"{CommonVariables.DebugFormat[gridManager.EntityNum]}Placing bunker. Iteration: {iterations}"); // Debug log output for testing
                iterations++; // Increments iterations for testing


                Tile tile = gridManager.Grid[r, c];

                tile.SetBunker(bunkerType);

                bunkerType.bunkerTiles[r*c] = tile;


            }
        }

        // Increments the player or ai full bunker count
        CommonVariables.BunkerCountsDictionary[gridManager.EntityNum].Set(CommonVariables.BunkerCountsDictionary[gridManager.EntityNum].Get() + 1); // Increments the corropsonding entities bunker count
    }

} // End of class

public class FullBunker
{
    // defines the attributes of a bunker
    private int rows;
    public int Rows
    { 
        get { return rows; }
        set { rows = value; TotalBunkers = Rows * Columns; } 
    }

    private int columns;
    public int Columns
    {
        get { return columns; }
        set { columns = value; TotalBunkers = Rows * Columns; }
    }

    public int TotalBunkers;
    public int NumberIdentifier;
    public Color BunkerColor;
    public GridManager GridManagerRef;

    public Tile[] bunkerTiles;

    public FullBunker(int givenRows, int givenColumns, Color givenColor, int givenIdentifier, GridManager givenGridManager) // Constructor to instantiate a bunker
    {
        Rows = givenRows;
        Columns = givenColumns;
        BunkerColor = givenColor;
        NumberIdentifier = givenIdentifier;
        GridManagerRef = givenGridManager;
        
        TotalBunkers = Rows * Columns; // Calculates total amount of bunker grid squares it takes up
        // will have bunker image in final iteration
    }
}
