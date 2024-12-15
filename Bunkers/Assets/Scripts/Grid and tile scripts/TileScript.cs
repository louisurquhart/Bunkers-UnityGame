using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Tile : MonoBehaviour
{ // ----------- START OF TILE CLASS -----------

    // need a box colider or something for click detection

    // References
    protected GridManager _gridManager;  // creates a reference to the gridmanager script
    public SpriteRenderer TileSpriteRenderer; // reference to the tiles sprite renderer to change its colour and stuff when hit

    // Variables - (private variables have _ infront to follow naming convention)
    protected int row; // row position of tile
    protected int col; // column position of the tile

    public int bunkerNumber;
    public int bunkerType;

    public bool IsBunker = false;  // checks if the tile has a bunker
    public bool IsRotated = false; // sets the rotation status of the tile (false by default)
    protected bool isPreviouslyHit = false;  // checks if tiles been hit

    protected string TileOwner = "Unknown"; // who owns the tile (AI/Player)


    protected void OnMouseDown() // Automatically called by unity if tile's clicked
    {
        OnTileClicked(); // Calls OnTileClicked function as tile has been clicked
    }
    protected void OnMouseOver() // Automatically called by unity if tile's hovered over
    {
        Color newColor = TileSpriteRenderer.color; // Gets the sprites current colour
        newColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
        TileSpriteRenderer.color = newColor; // Commits the new sprite colour with modified alpha (transparency)
    }
    protected void OnMouseExit() // Automatically called if tile's no longer being hovered over
    {
        Color newColor = TileSpriteRenderer.color; // Gets the sprites current colour
        newColor.a = 1f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
        TileSpriteRenderer.color = newColor; // Commits the new sprite colour with modified alpha (transparency)
    }

    // Initalise method to be called by the grid manager script when the grid's being created (creates an instance of this class per tile)
    public void Initalise(int rowRef, int colRef, GridManager gridManagerRef)
    // with the tiles position inputted row and collumn + a reference to the original grid manager script 
    {
        row = rowRef; // sets the classes row varaible to equal the inputted row from when called
        col = colRef; // sets the classes col variable to equal the inputted row from when called
        _gridManager = gridManagerRef; // sets the gridmanager variable == the inputted grid manager script
    }

    public virtual bool OnTileClicked()
    {
        return false;
    }

    public virtual void SetBunker(int bunkerNumber, Bunker bunkerType)
    {
    }

} // ---------------------- END OF TILE CLASS -----------------------------






// ----------- end of class -----------



