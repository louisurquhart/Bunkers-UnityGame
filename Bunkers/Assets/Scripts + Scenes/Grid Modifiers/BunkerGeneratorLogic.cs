using UnityEngine;

public class BunkerGenerator : ScriptableObject
{
    public static void RandomBunkerGenerator(GridManager gridManager) // random bunker generator. Will be used for the AI's grid + potentially the players if selected in options. Input 0 = Player, Input 1 = AI
    {
        FullBunker[] fullBunkers = gridManager.FullBunkers;

        for (int i = 0; i < fullBunkers.Length; i++) // Executes loop for the length of the bunker array (creates 1 of each bunkers)
        {
            FullBunker bunkerType = fullBunkers[i]; // Finds the bunker which will be added to the board in the loop iteration

            int randomRotation = UnityEngine.Random.Range(0, 2); // Generates a random rotation for the bunker. 0 = horizontal, 1 = vertical
            if (randomRotation == 0)
            {
                RotateBunker(bunkerType); // Generates a random rotation for the bunker
            }

            (int randomRow, int randomColumn, bool positionValidated) = generateRandomValidatedBunkerPosition(bunkerType, gridManager); // Generates a random position for the bunker

            if (positionValidated) // If the bunker randomly generated position has been validated
            {
                placeFullBunker(randomRow, randomColumn, gridManager, bunkerType); // Calls placeBunker to assign all the grid squares which the bunker takes up as bunkers
            }
            else
            {
                Debug.LogError($"{CommonVariables.DebugFormat[gridManager.EntityNum]}Failed to generate a random bunker position");
            }
        }
    }

    public static void ManualBunkerGenerator(int row, int column, PlayerGridManager gridManager)
    {
        FullBunker[] fullBunkers = gridManager.FullBunkers;

        if (gridManager.EntityNum != 0) // Validates entity is player not AI
        {
            // If it was called by AI grid manager it outputs an error and ends the procedure
            Debug.LogError("ManualBunkerGenerator called for AI");
            return;
        }
        else // If validation is successful
        {
            FullBunker fullBunker = fullBunkers[gridManager.placementIteration]; // Finds the bunker which will be added to the board in the loop iteration

            bool positionValidated = !DoesBunkerOverlap(row, column, fullBunker, gridManager); // Validates that bunker doesn't overlap (or is off grid)

            if (positionValidated) // If validation's successful
            {
                // Places the bunker + increases iteration of bunker being placed
                placeFullBunker(row, column, gridManager, fullBunker);
                gridManager.placementIteration++;

                // If all bunkers have been placed
                if (gridManager.placementIteration >= fullBunkers.Length) { CommonVariables.ManualBunkerPlacementActive = false; } // It stops manual bunker placement
            }
            else // If validation fails (which should've already been caught before being called)
            {
                // Warning's output to console + no action performed
                Debug.LogWarning("ManualBunkerGenerator: Bunker not placed as position unvalidated (should've already been caught!)");
            }
        }
    }

    // RandomBunkerGenerator sub methods (it's been refactored into smaller submethods for easier maintainability)
    protected static (int, int, bool) generateRandomValidatedBunkerPosition(FullBunker bunker, GridManager gridManager)
    {
        while (true) // Loop repeats until a position has been found
        {
            // Generates random bunker position
            int randomRow = Random.Range(0, 10 - bunker.Rows); // Generates a random row number
            int randomColumn = Random.Range(0, 10 - bunker.Columns); // Generates a random column number

            if (!DoesBunkerOverlap(randomRow, randomColumn, bunker, gridManager)) // Validates that the bunker won't overlap with any other bunkers
            {
                //Debug.Log("Bunker doesn't overlap so generating a bunker");
                return (randomRow, randomColumn, true); // sets external variable randomrow to the randomly generated row
            }
        }
    }

    // Method to check if a bunker will overlap if placed at a specific position (row + column)
    public static bool DoesBunkerOverlap(int row, int column, FullBunker fullBunker, GridManager gridManager) 
    {
        for (int r = row; r < row + fullBunker.Rows; r++)
        {
            for (int c = column; c < column + fullBunker.Columns; c++)
            {
                if (!(r >= 0 && r < gridManager.Grid.GetLength(0) && c >= 0 && c < gridManager.Grid.GetLength(1)) || gridManager.Grid[r, c].IsBunker) // If the position is a bunker it means it would be overlapping another bunker if placed, loop restarts as another position has to be found
                {
                    //Debug.Log($"INFO: Overlap found at position: Row = {row} Column = {column}");
                    return true; // If a bunker is found in the way, true is returned as the would be position overlaps.
                }
            }
        }
        return false; // If the for loops go through all the bunker positons and finds no bunkers in the way, false is returned to signify there's no overlap
    }

    // Method to randomly rotate a bunker to a random rotation (either horizontal or vertical)
    public static void RotateBunker(FullBunker bunker)
    {
        int tempRows = bunker.Rows;
        bunker.Rows = bunker.Columns; // rows are set equal to columns
        bunker.Columns = tempRows; // columns are set equal to rows
        // Bunkers rotation's vertical by default so no need to change rows + columns around if it's vertical
    }

    private static void placeFullBunker(int row, int column, GridManager gridManager, FullBunker bunkerType)
    {
        bunkerType.BaseRow = row;
        bunkerType.BaseCol = column;

        // -- Calculates the total amount of rows and columns the bunker will take up --
        int finalRow = row + bunkerType.Rows;
        int finalColumn = column + bunkerType.Columns;

        int iterations = 0;

        // -- Code for testing --
        //int totalBunkersPlaced = bunkerType.Rows * bunkerType.Columns; // TotalBunkersPlaced calculated for debug log (testing)
        //Debug.Log($"<b>{CommonVariables.DebugFormat[gridManager.EntityNum]}Placing full bunker (Number: {bunkerType.NumberIdentifier}). Total bunkers placed should be {totalBunkersPlaced}"); // Debug log output for testing

        
        // -- Nested for loops go through every tile the bunker occupies and designated them as bunker tiles:
        for (int r = row; r < finalRow ; r++) // Goes over every row the bunker needs to occupy
        {
            for (int c = column; c <  finalColumn ; c++) // For every row it gom es through every column on that row that the bunker needs to occupy.
            {
                // Code for testing
                //Debug.Log($"{CommonVariables.DebugFormat[gridManager.EntityNum]}Placing bunker. Iteration: {iterations + 1}"); // Debug log output for testing

                Tile tile = gridManager.Grid[r, c]; // Finds the tile in the position through the grid manager array

                tile.SetBunker(bunkerType); // Sets the tile as a bunker

                iterations++; // Increments iterations 
            }
        }

        // Increments the player or ai full bunker count
        CommonVariables.BunkerCountsDictionary[gridManager.EntityNum].Set(CommonVariables.BunkerCountsDictionary[gridManager.EntityNum].Get() + 1); // Increments the corropsonding entities bunker count
        //Debug.Log($"AI TotalFullBunkerCount: {CommonVariables.AIAliveFullBunkerCount}, Player TotalFullBunkerCount: {CommonVariables.PlayerAliveFullBunkerCount}");
    }

} // End of class




