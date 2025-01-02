using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Tile : MonoBehaviour
{ // ----------- START OF TILE CLASS -----------

    // need a box colider or something for click detection

    // References
    public GridManager GridManager;  // creates a reference to the gridmanager script
    public SpriteRenderer TileSpriteRenderer; // reference to the tiles sprite renderer to change its colour and stuff when hit

    // Variables - (private variables have _ infront to follow naming convention)
    public int row; // row position of tile
    public int col; // column position of the tile

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
                Debug.LogError($"FullBunkerReference not returned as isBunker == {IsBunker}");
                return null;
            }
        }
        set { fullBunkerReference = value; }
    }

    public bool IsBunker = false;  // checks if the tile has a bunker
    public bool IsRotated = false; // sets the rotation status of the tile (false by default)

    [SerializeField] private bool isPreviouslyHit;
    public bool IsPreviouslyHit
    {
        get { return isPreviouslyHit; }
        set 
        {
            Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}isPreviouslyHit on tile row: {row} column: {col} set to: {value}");
            isPreviouslyHit = value;
        }
    }

    public string TileOwner; // who owns the tile (AI/Player)


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
        GridManager = gridManagerRef; // sets the gridmanager variable == the inputted grid manager script
    }

    public virtual void SetBunker(FullBunker givenBunkerType)
    {
        Debug.LogError($"{CommonVariables.DebugFormat[GridManager.EntityNum]}SetBunker: Failiure to override SetBunker method");
    }

} // ---------------------- END OF TILE CLASS -----------------------------






// ----------- end of class -----------



