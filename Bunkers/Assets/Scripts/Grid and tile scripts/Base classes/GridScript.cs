using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class GridManager : MonoBehaviour
{
    // Variables - (private variables have _ infront to follow naming convention)
    public int _rows = 9;     // number of rows for the grid
    public int _colums = 9;     // number of columns for the grid 
    protected int bunkerNumber; // Number of the full bunker it's apart of

    [SerializeField] protected bool isPlayer = false;
    [SerializeField] protected GameObject _tileprefab;  // tile prefab (editable copy of a tile) linked
    [SerializeField] protected Transform _gridObject;   // creates a parent called gridobject for all the tiles to be put under so they can be
                                                        // anchored and moved together + organised under 1 parent 

    public Tile[,] Grid;  // array to store the tiles

    void Start()
    {
        CreateGrid();
    }

    // Function to create the players own grid at the start of the game 
    // (not AI's might need a different script or to repurpose this one to make it work for it)
    public void CreateGrid()
    {
        Debug.Log("CreateGrid has been called");

        Grid = new Tile[_rows, _colums]; // creates an array for the grid to store the tiles

        for (int row = 0; row < _rows; row++)  // goes through every row for the grid (row = 0 until row == max rows - 1)
        {
            for (int column = 0; column < _colums; column++) // goes through every collum for the selected row
            {
                Debug.Log("Creating a tile at Row: " + row + " Column: " + column);
                GameObject newTile = Instantiate(_tileprefab, _gridObject); //instantiates (creates) a new tile prefab and puts it under the parent gridobject

                newTile.transform.localPosition = new Vector2(column * 0.11f, -row * 0.11f); // using the transform component of the newly created tile it positions it
                                                                                         // relative to its parent by its column * number + row * number to make sure it's
                                                                                        // spaced and alligned in its row and relative to other tiles
      
                Tile tileScript = newTile.GetComponent<Tile>(); // creates a new variable (tilescript) to reference the script attached to the new tile
                                                                // GetComponent gets the tilescript which is on the tile

                tileScript.Initalise(row,column,this); // calls the initialise function inside the tilescript
                                                        // and inputs the row + column it is located + this class

                Grid[row, column] = tileScript; // stores the tilescript into the grid array
                                               // for working with later (identifying if bunker, setting if hit, ect)
            }
        }
        Debug.Log($"Calling random bunker generator. Class name: {this}");
        BunkerGenerator.RandomBunkerGenerator(isPlayer, this); // After grid has been fully generated, it calls the RandomBunkerGenerator 
    }

    public virtual void OnBunkerHit(int row, int col)
    {

    }

    public virtual void OnBunkerMiss(int row, int col) // function called when a tile's hit but there's no bunker on the tile (validation already done)
    {
        Debug.LogError("OnBunkerMiss: Method hasn't been overridden. No code is executed.");
    }

    public void DestroyGrid()
    {
        Destroy(_gridObject.gameObject);
    }

}