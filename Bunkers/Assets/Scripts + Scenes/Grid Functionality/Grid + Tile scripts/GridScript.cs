using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI.Table;

public class GridManager : MonoBehaviour
{
    // Variables - (private variables have _ infront to follow naming convention)
    public int _rows = 9; // number of rows for the grid
    public int _colums = 9;  // number of columns for the grid 
    public bool isPlayer;
    public int EntityNum;

    // Hit + Miss color values. Will be replaced with images in final iteration
    [SerializeField] protected UnityEngine.Color _missColour;
    [SerializeField] protected UnityEngine.Color _hitColour;

    [SerializeField] protected BunkerGenerator _bunkerGenerator;
    [SerializeField] protected GameObject _tileprefab;  // tile prefab (editable copy of a tile) linked
    [SerializeField] protected Transform _gridObject;   // creates a parent called gridobject for all the tiles to be put under so they can be
                                                        // anchored and moved together + organised under 1 parent 

    public Tile[,] Grid;  // array to store the tiles

    private void Awake()
    {
        if(isPlayer)
        {
            EntityNum = 0;
        }
        else
        {
            EntityNum = 1;
        }
    }

    void Start()
    {
        CreateGrid();
    }

    // Function to create the players own grid at the start of the game 
    // (not AI's might need a different script or to repurpose this one to make it work for it)
    public void CreateGrid()
    {
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}CreateGrid has been called");

        Grid = new Tile[_rows, _colums]; // creates an array for the grid to store the tiles

        for (int row = 0; row < _rows; row++)  // goes through every row for the grid (row = 0 until row == max rows - 1)
        {
            for (int column = 0; column < _colums; column++) // Goes through every collum for the selected row
            {
                //Debug.Log("Creating a tile at Row: " + row + " Column: " + column);
                GameObject newTile = Instantiate(_tileprefab, _gridObject); // Instantiates (creates) a new tile prefab and puts it under the parent gridobject

                newTile.transform.localPosition = new Vector2(column * 0.11f, -row * 0.11f); // Using the transform component of the newly created tile it positions it
                                                                                         // relative to its parent by its column * number + row * number to make sure it's
                                                                                        // spaced and alligned in its row and relative to other tiles
      
                Tile tileScript = newTile.GetComponent<Tile>(); // Creates a new variable (tilescript) to reference the script attached to the new tile
                                                                // GetComponent gets the tilescript which is on the tile

                tileScript.Initalise(row,column,this); // Calls the initialise function inside the tilescript
                                                        // and inputs the row + column it is located + this class

                Grid[row, column] = tileScript; // stores the tilescript into the grid array
                                               // for working with later (identifying if bunker, setting if hit, ect)
            }
        }
        //Debug.Log($"Calling random bunker generator. Class name: {this}");

        _bunkerGenerator.RandomBunkerGenerator(); // After grid has been fully generated, it calls the RandomBunkerGenerator 
    }

    public int OnTileHit(Tile tile) // OnTileHit function. Does immediate validation of hit
    {
        if (!tile.IsPreviouslyHit) // makes sure the tile hasn't been hit before + validating it's the entities turn
        {
            Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}Tile hit at: Row: {tile.row} Column: {tile.col}"); // outputs into the unity console that a tile has been detected to have been clicked. (for  testing purposes)

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
        else if (tile.IsPreviouslyHit) // The tile has already been hit
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

        tile.TileSpriteRenderer.color = _hitColour; // Changes the tiles colour to red (will be a sprite in final iteration just for prototype & testing)

        DecrementBunkerCount(tile);
    }

    private void DecrementBunkerCount(Tile tile)
    {
        FullBunker fullBunker = tile.FullBunkerReference;

        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}DecrementBunkerCount: Bunker: {tile.FullBunkerReference.NumberIdentifier} count decremented from: {fullBunker.TotalBunkers} to: {fullBunker.TotalBunkers-1}.");
        fullBunker.TotalBunkers--;

        if (fullBunker.TotalBunkers <= 0) // If the bunker's completely destroyed (has no small bunkers left in it)
        {
            Debug.Log($"{CommonVariables.DebugFormat[EntityNum]}DecrementBunkerCount: Full bunker destroyed");
            DestroyFullBunker(fullBunker);
        }
    }

    private void DestroyFullBunker(FullBunker fullBunker)
    {
        


        // Decrements full bunker count
        if (EntityNum == 0)
        {
            CommonVariables.PlayerAliveBunkerCount--;
        }
        else
        {
            CommonVariables.AIAliveBunkerCount--;
        }

        GeneralBackgroundLogic.HasGameEnded(); // Calls HasGameEnded function to check if this hit was gameending. It will deal with all outcomes

        // Potentially change all bunker elements colour to signify it's completely destroyed
    }

    public void OnBunkerMiss(Tile tile)
    {
        Debug.Log($"{CommonVariables.DebugFormat[EntityNum]} OnBunkerMiss: Determined tile wasn't a bunker"); // outputs into the unity console that it's identified cell has been clicked. (for debugging + testing purposes)

        // In future will need to check if the tile's a special bunker here (special bunkers will be implemented in final prototype)

        tile.TileSpriteRenderer.color = _missColour; // changes the tiles colour to blue to indicate miss (will be a sprite in final iteration just for prototype & testing)

        // Hit animation will be added in the final iteration
        // Sound effect will be added in final iteration


        // Update statistics when they're implemented (enemy bunkers hit + maybe special bunker hit too)
    }

    public void DestroyGrid()
    {
        Destroy(_gridObject.gameObject);
    }



}