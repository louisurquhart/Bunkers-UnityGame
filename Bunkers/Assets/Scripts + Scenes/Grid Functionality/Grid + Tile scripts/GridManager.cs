using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI.Table;

public class GridManager : MonoBehaviour
{
    // Variables - (private variables have _ infront to follow naming convention)
    protected int _rows = 9; // number of rows for the grid
    protected int _colums = 9;  // number of columns for the grid 
    public int EntityNum;

    // Hit + Miss color values. Will be replaced with images in final iteration
    [SerializeField] protected Color _missColour;
    [SerializeField] protected Color _hitColour;
    [SerializeField] public Color _defaultColour;

    // References to gameobject/component/other script instance set in inspector:
    [SerializeField] protected GameObject _tileprefab; // Reference to the corrosponding tile prefab for the entity
    [SerializeField] protected Transform _gridObject; // Reference to GridManagers parent GameObject (where the tiles positon will be based off)
    [SerializeField] protected BunkerGenerator BunkerGenerator;

    public Tile[,] Grid;  // Array to store the tiles in the grid (referenced by position [row, column])

    public FullBunker[] FullBunkers = new FullBunker[5]; // makes an array storing different types of bunkers
    protected void Awake() // Called immediately after class instantiation
    {
        // FullBunker classes are instantiated with given values (so they can be generated easily using a for loop + their properties are easily modifiable)
        FullBunkers[0] = new FullBunker(3, 1, Color.green, this); // 0th position in array
        FullBunkers[1] = new FullBunker(4, 1, Color.red, this);// 1st position in array
        FullBunkers[2] = new FullBunker(5, 1, Color.magenta, this); // 2nd position in array
        FullBunkers[3] = new FullBunker(2, 2, Color.gray, this); // 3rd position in array
        FullBunkers[4] = new FullBunker(6, 1, Color.yellow, this); // 4th position in array
    }

    void Start() // Called after awake
    {
        CreateGrid(); // Calls create grid to start the formation of grid as soon as scene's loaded
    }

    // Function to create the players own grid at the start of the game 
    public void CreateGrid()
    {
        Debug.Log($"<b>{CommonVariables.DebugFormat[EntityNum]}CreateGrid has been called"); // Log for testing

        Grid = new Tile[_rows, _colums]; // Creates an array for the grid to store the tiles

        for (int row = 0; row < _rows; row++)  // Goes through every row for the grid (row = 0 until row == max rows - 1)
        {
            for (int column = 0; column < _colums; column++) // Goes through every collum for the selected row
            {
                GameObject newTile = Instantiate(_tileprefab, _gridObject); // Instantiates (creates) a new tile prefab and puts it under the parent gridobject

                newTile.transform.localPosition = new Vector2(column * 0.11f, -row * 0.11f); // Using the transform component of the newly created tile, it's positioned relative to its parent 
                                                                                             // by its column * number + row * number so it's spaced + alligned in a perfect grid
                Tile tileScript = newTile.GetComponent<Tile>(); // Creates a reference to the tile's script attached to it

                tileScript.Initalise(row, column, this); // Initializes the tileScript with its row + column
                
                Grid[row, column] = tileScript; // Stores the tilescript into the grid array
                                                // for working with later (identifying if bunker, setting if hit, ect)
                                                //Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}New tile added at row: {row}, column: {column}. GridArray tile value: {Grid[row, column]}"); // Debug log for testing
            }
        }

        if (PlayerPrefs.GetInt("ManualBunkerPlacementActive", 0) == 0 && EntityNum == 0) // If manual bunker placement's active and the entity == player
        {
            CommonVariables.ManualBunkerPlacementActive = true; return; // Sets ManualBunkerPlacement == active + ends procedure
        }
        else { BunkerGenerator.RandomBunkerGenerator(this); } // If manual bunker placement's not active or entity isn't player it ends the procedure
    }

    // OnTileHit function called when the AI chooses a tile to hit or a player clicks on a tile
    public int OnTileHit(Tile tile) 
    {
        // Validates bunker placement isn't active
        if (CommonVariables.ManualBunkerPlacementActive)
        { return 2; }

        if (!tile.IsPreviouslyHit) // makes sure the tile hasn't been hit before + validating it's the entities turn
        {
            Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}Tile: {tile} hit at: Row: {tile.Row} Column: {tile.Col}"); // outputs into the unity console that a tile has been detected to have been clicked. (for  testing purposes)

            tile.IsPreviouslyHit = true; // sets the isHit variable to true to signifiy it has already been clicked

            if (tile.IsBunker) // If tile's a bunker 
            {
                OnBunkerHit(tile); // it calls the onhit function in the gridmanager script inputting its row and column as variables
                GeneralBackgroundLogic.ChangeTurn();
                return 0; // returns true to signify that the method completed successfully 
            }
            else // if tile's not a bunker 
            {
                OnBunkerMiss(tile); // it calls the onmiss function in the gridmanager script inputting its row and column as variables
                GeneralBackgroundLogic.ChangeTurn();
                return 0; // returns true to signify that the method completed successfully
            }
            
        }
        else if (tile.IsPreviouslyHit) // If the tile has already been hit
        {
            return 1; // Returns 1 to signify that the method failed to complete as tile's already hit
        }
        else // Its not the entities turn
        { 
            return 2; // Returns 2 to signifiy method failed to complete as it's not the entities turn
        }
    }

    public void OnBunkerHit(Tile tile) // function called when a tile with a bunkers been hit (validation already done)
    {
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}OnBunkerHit: Determined tile was a bunker"); // outputs into the unity console that it's identified cell has been clicked. (for debugging + testing purposes)

        tile.UpdateTileColour(_hitColour, false); // Changes the tiles colour to red (false input to signify not temporary)

        decrementBunkerCount(tile);

        // Statistic incrementation
        if (EntityNum == 1) { StatisticsMenuFunctionality.IncrementStatisticValue("TotalNumberOfHits"); } // If it was the AI grid hit, it incrmenets the players totalNumberOfHits statistic
    }

    public void OnBunkerMiss(Tile tile)
    {
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]} OnBunkerMiss: Determined tile wasn't a bunker"); // outputs into the unity console that it's identified cell has been clicked. (for debugging + testing purposes)

        tile.UpdateTileColour(_missColour, false); // Changes the tiles colour to blue to indicate miss (false input to signify not temporary)

        // Statistic incrementation
        if (EntityNum == 1) { StatisticsMenuFunctionality.IncrementStatisticValue("TotalNumberOfMisses"); } // If it was the AI grid hit, it incrmenets the players totalNumberOfHits statistic
    }


    // ---------- Private methods created from refactoring OnBunkerHit to improve maintainability ---------
    private void decrementBunkerCount(Tile tile)
    {
        FullBunker fullBunker = tile.FullBunkerReference;

        fullBunker.TotalAliveBunkers--; // Decrements the fullBunkers total bunker count
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}DecrementBunkerCount: Total bunker count for the fullBunker: {fullBunker} decreased to: {fullBunker.TotalAliveBunkers}"); // Outputs message for debugging

        // Checks if bunker's completely destroyed
        if (fullBunker.TotalAliveBunkers <= 0) // If the bunker's completely destroyed (has no small bunkers left in it)
        {
            Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}DecrementBunkerCount: FullBunker completely destroyed");
            destroyFullBunker(fullBunker); // If so it calls DestroyFullBunker
        }
    }

    private void destroyFullBunker(FullBunker fullBunker) // Method called if a fullBunker has no bunkers left, changes colour of all tiles in it + decrements entities global bunker count
    {
        // Decrements entities full bunker count:
        CommonVariables.BunkerCountsDictionary[EntityNum].Set(CommonVariables.BunkerCountsDictionary[EntityNum].Get() - 1);
        Debug.Log($"<b>{CommonVariables.DebugFormat[EntityNum]}New FullBunker count: {CommonVariables.BunkerCountsDictionary[EntityNum].Get()}");
        // --- Changes all bunker elements colours to the fullBunkers colour + more transparent (To show the player that they've destroyed a full bunker or their full bunkers been destroyed):
        Color newColor = fullBunker.BunkerColor; // Gets the sprites current colour
        newColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 80% (more transparent)
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}Full bunker base row: {fullBunker.BaseRow}, base col: {fullBunker.BaseCol}, fullBunker rows: {fullBunker.Rows}, fullBunker columns: {fullBunker.Columns}");
        UpdateFullBunkerTilesColour(newColor, fullBunker, fullBunker.BaseRow, fullBunker.BaseCol, false); // Sets all the tiles colour to their fullBunkers colour

        // Updates statistics:
        StatisticsMenuFunctionality.IncrementStatisticValue("FullBunkersDestroyed");

        // Checks if game has ended (as this could be a game ending change):
        GeneralBackgroundLogic.HasGameEnded(); 
    }

    public void UpdateFullBunkerTilesColour(Color? newColor, FullBunker fullBunker, int row, int col, bool temporary)
    {
        int finalRow = row + fullBunker.Rows;
        int finalCol = col + fullBunker.Columns;

        for (int r = row; r < finalRow; r++) // Goes through every tile in the full bunker
        {
            for (int c = col; c < finalCol; c++)
            {
                if (r >= 0 && r < Grid.GetLength(0) && c >= 0 && c < Grid.GetLength(1) && Grid[r,c].IsBunker == !temporary)
                {
                    if (newColor.HasValue)
                    {
                        Grid[r, c].UpdateTileColour(newColor.Value, temporary);
                    }
                    else
                    {
                        Debug.Log($"UpdateFullBunker: Updating tile to its default colour: {Grid[r, c].TileColour}");
                        Grid[r, c].UpdateTileColour(Grid[r, c].TileColour, temporary);
                    }
                }
            }
            //fullBunker.bunkerTilesArray[i].UpdateTileColour(newColor); // Calls UpdateTileColour with the colour
        }
    }

    virtual public void TileOnMouseOver(Tile tile)
    {
        Color tempColor = tile.TileColour;  // Gets the sprites current colour
        tempColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
        tile.TileSpriteRenderer.color = tempColor;
    }

}