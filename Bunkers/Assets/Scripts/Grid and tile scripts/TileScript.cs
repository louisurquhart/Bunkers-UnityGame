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
    public int row; // row position of tile
    public int col; // column position of the tile

    public int bunkerNumber;
    public int bunkerType;


    public bool IsBunker = false;  // checks if the tile has a bunker
    public bool IsRotated = false; // sets the rotation status of the tile (false by default)
    public bool isPreviouslyHit = false;  // checks if tiles been hit

    public string TileOwner = "Unknown"; // who owns the tile (AI/Player)


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

    public bool OnTileHit() // Override method of OnTileHit function. Does immediate validation of hit
    {
        if (!isPreviouslyHit && !CommonVariables.PlayerTurn) // makes sure the tile hasn't been hit before + validating it's definitely the AI's turn
        {
            Debug.Log($"{TileOwner} owned tile hit at: Row = {row} Column = {col}"); // outputs into the unity console that a tile has been detected to have been clicked. (for  testing purposes)

            isPreviouslyHit = true; // sets the isHit variable to true to signifiy it has been clicked (incase it's clicked again)

            if (IsBunker == true) // if tile's a bunker 
            {
                _gridManager.OnBunkerHit(this); // it calls the onhit function in the gridmanager script inputting its row and column as variables
                return true; // returns true to signify that the method completed successfully
            }
            else // if tile's not a bunker 
            {
                _gridManager.OnBunkerMiss(this); // it calls the onmiss function in the gridmanager script inputting its row and column as variables
                return true; // returns true to signify that the method completed successfully
            }
        }
        else if (isPreviouslyHit) // If the tile has been hit before 
        {
            Debug.Log("OnTileClicked: Entity tried to hit a tile which has already been hit"); // Outputs error that that validation blocked the player trying to hit a tile when it's not their turn
            return false; // returns false to signify that the method failed to complete successfully due to one or more validation failiures
        }
        else // Else if it's not been previously hit it must not be the players turn 
        {
            Debug.Log("OnTileClicked: Entity tried to hit a tile which has already been hit"); // outputs info message that validation blocked the player from clicking a tile when it's not their turn for debugging + testing
            return false; // Returns false to signify that the method failed to complete successfully due to one or more validation failiures
        }
    }

    public virtual void SetBunker(int bunkerNumber, Bunker bunkerType)
    {
    }

} // ---------------------- END OF TILE CLASS -----------------------------






// ----------- end of class -----------



