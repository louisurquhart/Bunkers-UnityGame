using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Tile : MonoBehaviour
{ 
    // ------- Variables --------
    public int row; // row position of tile
    public int col; // column position of the tile

    // Tile state variables
    public bool IsBunker = false;  // checks if the tile has a bunker
    public bool IsRotated = false; // sets the rotation status of the tile (false by default)


    // ------- Encapsulated properties for the tiles attributes --------

    // GridManager property
    private GridManager gridManager;
    public GridManager GridManager
    {
        get { return gridManager; } // Only has getter to make it read only to other classes as only intialize method in the class should modify it.
    }

    // TileSpriteRenderer property 
    [SerializeField] private SpriteRenderer tileSpriteRenderer;
    public SpriteRenderer TileSpriteRenderer
    {
        get { return tileSpriteRenderer; } // Only has getter to make it read only to other classes as only intialize method in the class should modify it.
    }

    // TileColour property
    private Color tileColour;
    public Color TileColour
    {
        get{ return tileColour; }
        set 
        { 
            if (value == TileSpriteRenderer.color) {tileColour = value;} // If the value TileColour is being to set to is actually the tile's colour it sets it
            else{ Debug.LogWarning("Given tileColour not synchronized with actual tile colour. (tileColour = {tileColour} actualTileColour = {TileSpriteRenderer.color. No changes made "); } // Otherwise it outputs a warning saying that the input wasn't accepted
        }

    }

    // FullBunkerReference property
    private FullBunker fullBunkerReference;
    public FullBunker FullBunkerReference 
    {
        get 
        {
            if (IsBunker) { return fullBunkerReference; } // If the tile is a bunker it returns the reference to the fullBunker
            else { Debug.LogError($"FullBunkerReference not returned. isBunker == {IsBunker}"); return null; }// Otherwise it returns null as if the tile isn't a bunker it can't have a fullBunker reference + outputs error to signify this} 
        }
        set { fullBunkerReference = value; }
    }

    // IsPreviouslyHit property
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

    protected void Awake() // Called immediately after class is created
    {
        // Syncronises the TileColour variable with the tile's actual colour
        TileColour = TileSpriteRenderer.color;
    }


    // --- Procedures to highlight tile when it's hovered over ---
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


    // Initialise method called by gridManager to create + set the properties of the tile
    public void Initalise(int rowRef, int colRef, GridManager gridManagerRef)
    {
        // Sets corrosponding variables to the given ones
        row = rowRef; 
        col = colRef; 
        gridManager = gridManagerRef; 
        tileSpriteRenderer = GetComponent<SpriteRenderer>();

        // Outputs log for testing
        Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}Initialise: Tile {this} at row: {rowRef}, {colRef} initialized. Rows == {row}, Columns == {col}, TileSpriteRenderer == {TileSpriteRenderer}");
    }

    // Method to set the tile as bunker (should be overrided by tile subclasses as it depends on which entities tile it is)
    public virtual void SetBunker(FullBunker givenBunkerType)
    {
        Debug.LogError($"{CommonVariables.DebugFormat[GridManager.EntityNum]}SetBunker: Failiure to override SetBunker method"); // If method's not overrided it outputs error
    }


    // Method to change the tiles colour (should change tileColour value + the actual tiles colour)
    public void UpdateTileColour(Color color)
    {
        tileColour = color;
        Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}UpdateTileColour: Updating tile: {this} at row: {row}, column: {col} colour to: {color}");
        TileSpriteRenderer.color = color;
    }
}






