using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class BunkerGenerator : MonoBehaviour
{

    static int[] bunkerCount = new int[] { 0, 1, 2, 3, 3 }; // amount of each type of bunker which will be put on the grid

    static  int length = bunkerCount.Length;

    static Bunker[] bunkerArray = new Bunker[] // makes an array storing different types of bunkers
    {
        new Bunker("squareBunker", 2, 2, Color.green), // 0th position in array
        new Bunker("longBunker", 4, 1, Color.red), // 1st position in array
        new Bunker("mediumBunker", 3, 1, Color.magenta), // 2nd position in array
        new Bunker("shortBunker", 2, 1, Color.gray) // 3rd position in array
    };

    public static void RandomBunkerGenerator(bool isPlayer, GridManager gridManager) // random bunker generator. Will be used for the AI's grid + potentially the players if selected in options. Input 0 = Player, Input 1 = AI
    {
        Debug.Log("INFO: RandomBunkerGenerator has been called");

        for (int i = 0; i < length; i++) // Executes loop 
        {
            //Debug.Log($"Executing for loop. Iteration: {i}");

            int bunkerRows;
            int bunkerColumns;

            Bunker bunkerType = bunkerArray[bunkerCount[i]]; // finds the bunker which needs to be placed in the array

            // ------------ Random bunker rotation generator ------------

            int randomRotation = UnityEngine.Random.Range(0, 2); // Generates a random rotation for the bunker. 0 = horizontal, 1 = vertical

            if (randomRotation == 0) // If the rotation is horizontal (0) it switches the rows + columns around. By default they're vertical so no changing needs to be done then
            {
                int temprows = bunkerType.Rows; // sets a temporary variable to == rows so rows can be set to columns and the value isn't lost
                bunkerRows = bunkerType.Columns; // rows are set equal to columns
                bunkerColumns = temprows; // columns are set equal to rows
            }
            else if (randomRotation == 1)
            {
                bunkerRows = bunkerType.Rows;
                bunkerColumns = bunkerType.Columns;
            }
            else
            {
                Debug.LogError($"ERROR - RandomBunkerGeneration: Given rotation is out of bounds (0-1). randomRotation == {randomRotation}. Ending bunker generation");
                break;
            }

            // ------------ Random bunker position generaton + validation of position ------------
            
            int randomRow = 0;
            int randomColumn = 0;
            bool positionValidated = false;
            int iterations = 0;

            while (positionValidated == false && iterations < 10000) // Loop repeats until a position has been found or if it has repeated 10,000 times which means that there's an issue with the code
                                                                     // (iteration check is done for testing to prevent infinite loop + softlock)
            {
                // Generates random bunker position
                int unvalidatedRandomRow = UnityEngine.Random.Range(0, 10 - bunkerRows); // Generates a random row number
                int unvalidatedRandomColumn = UnityEngine.Random.Range(0, 10 - bunkerColumns); // Generates a random column number

                if (!doesBunkerOverlap(unvalidatedRandomRow, unvalidatedRandomColumn, bunkerRows, bunkerColumns, gridManager)) // First validates that the bunker won't overlap with any other bunkers
                {
                    //Debug.Log("Bunker doesn't overlap so generating a bunker");
                    randomRow = unvalidatedRandomRow; // sets external variable randomrow to the randomly generated row
                    randomColumn = unvalidatedRandomColumn; // sets external variable randomcolumn to the randomly generated column
                    positionValidated = true;
                    break;
                }
                //Debug.Log($"Bunker overlaps, Retrying");
                iterations++;
            }
            if (positionValidated == true)
            {
                placeBunker(randomRow, randomColumn, gridManager, bunkerRows, bunkerColumns, i, bunkerType); // Calls the place bunker procedure after the position has been generated + validated
            }
        } // End of for loop

    } // End of RandomBunkerGeneration method

    private static bool doesBunkerOverlap(int row, int column, int bunkerRows, int bunkerColumns, GridManager gridManager)
    {
        //Debug.Log("Checking if bunker overlaps");
        for (int r = row; r < row + bunkerRows; r++)
        {
            for (int c = column; c < column + bunkerColumns; c++)
            {
                if (gridManager.Grid[r, c].IsBunker) // If the position is a bunker it means it would be overlapping another bunker if placed, loop restarts as another position has to be found
                {
                    Debug.Log($"INFO: Overlap found at position: Row = {row} Column = {column}");
                    return true; // If a bunker is found in the way, true is returned as the would be position overlaps.
                }
            }
        }

        return false; // If the for loops go through all the bunker positons and finds no bunkers in the way, false is returned to signify there's no overlap
    }
    private static void placeBunker(int row, int column, GridManager gridManager, int bunkerRows, int bunkerColumns, int bunkerNumber, Bunker bunkerType)
    {
        //Debug.Log($"INFO: PlaceBunker has been called. Inputted properties: Positon - Row: {row}, Column: {column}. Bunker size - Rows = {bunkerType.Rows}, Columns = {bunkerType.Columns} ");

        int finalRow = row + bunkerRows;
        int finalColumn = column + bunkerColumns;
        Debug.Log($"finalRow = {finalRow}, finalColumn = {finalColumn}");

        for (int r = row; r < finalRow ; r++)
        {
            for (int c = column; c <  finalColumn ; c++)
            {
                gridManager.Grid[r, c].SetBunker(bunkerNumber, bunkerType);
                Debug.Log($"Placing a bunker at row = {r}, column = {c}");
            }
        }
    }

} // end of class
