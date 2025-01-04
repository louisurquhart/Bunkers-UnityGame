using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Tile : MonoBehaviour
{ // ----------- START OF TILE CLASS -----------

    // Variables - (private variables have _ infront to follow naming convention)
    public int row; // row position of tile
    public int col; // column position of the tile

    // Tile state variables
    public bool IsBunker = false;  // checks if the tile has a bunker
    public bool IsRotated = false; // sets the rotation status of the tile (false by default)

    // References
    public GridManager GridManager;  // creates a reference to the gridmanager script

    private SpriteRenderer tileSpriteRenderer;
    public SpriteRenderer TileSpriteRenderer // reference to the tiles sprite renderer to change its colour and stuff when hit
    {
        get { return tileSpriteRenderer; }
    }


    private Color tileColour;
    public Color TileColour
    {
        get{ return tileColour; }

        set
        {
            if (value == TileSpriteRenderer.color)
            {
                tileColour = value;
            }
            else
            {
                Debug.LogWarning("Given tileColour not synchronized with actual tile colour. (tileColour = {tileColour} actualTileColour = {TileSpriteRenderer.color ");
            }
        }

    }

    private FullBunker fullBunkerReference;
    public FullBunker FullBunkerReference 
    {
        get 
        { 
            if (IsBunker) 
            { 
                return fullBunkerReference; 
            } 
            else
            {
                Debug.LogError($"FullBunkerReference not returned. isBunker == {IsBunker}");
                return null;
            }
        }
        set { fullBunkerReference = value; }
    }
    private bool isPreviouslyHit;
    public bool IsPreviouslyHit
    {
        get { return isPreviouslyHit; }
        set 
        {
            Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}isPreviouslyHit on tile row: {row} column: {col} set to: {value}");
            isPreviouslyHit = value;
        }
    }

    // Method called by GridManager to initialize the tile with its row + column
    public void Initalise(int rowRef, int colRef, GridManager gridManagerRef)
    // with the tiles position inputted row and collumn + a reference to the original grid manager script 
    {
        row = rowRef; // sets the classes row varaible to equal the inputted row from when called
        col = colRef; // sets the classes col variable to equal the inputted row from when called
        GridManager = gridManagerRef; // sets the gridmanager variable == the inputted grid manager script
        tileSpriteRenderer = GetComponent<SpriteRenderer>(); // Sets the tiles sprite renderer to the one attached to it
        Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}Initialise: Tile {this} at row: {rowRef}, {colRef} initialized. Rows == {row}, Columns == {col}, TileSpriteRenderer == {TileSpriteRenderer}");
    }

    protected void Start()
    {
        TileColour = TileSpriteRenderer.color;
    }

    private void OnDestroy()
    {
        Debug.Log("<b>{CommonVariables.DebugFormat[GridManager.EntityNum]}Tile destroyed");
    }

    protected void OnMouseOver() // Automatically called by unity if tile's hovered over. Temporaraly makes the tile more transparent
    {
        Color tempColor = TileColour;  // Gets the sprites current colour
        tempColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
        TileSpriteRenderer.color = tempColor; // Commits the new sprite colour with modified alpha (transparency)
    }

    protected void OnMouseExit() // Automatically called if tile's no longer being hovered over. Sets the tile back to its regular colour
    {
        TileSpriteRenderer.color = TileColour;
    }

    public void UpdateTileColour(Color color)
    {
        tileColour = color;
        Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}UpdateTileColour: Updating tile: {this} at row: {row}, column: {col} colour to: {color}");
        TileSpriteRenderer.color = color;
    }

    // Initalise method to be called by the grid manager script when the grid's being created (creates an instance of this class per tile)

    public virtual void SetBunker(FullBunker givenBunkerType)
    {
        Debug.LogError($"{CommonVariables.DebugFormat[GridManager.EntityNum]}SetBunker: Failiure to override SetBunker method");
    }

} // ---------------------- END OF TILE CLASS -----------------------------






// ----------- end of class -----------



