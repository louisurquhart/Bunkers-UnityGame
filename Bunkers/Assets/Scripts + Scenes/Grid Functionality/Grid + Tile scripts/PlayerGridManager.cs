using UnityEngine;

public class PlayerGridManager : GridManager
{
    void Start()
    {
        CreateGrid(); // Calls create grid to start the formation of grid as soon as scene's loaded
    }

    public int PlacementIteration; // Variable to store iteration of which bunker's being placed for indexing

    // Property to store the current full bunker being placed -> needs to be publically accessible for visualisation on board screen when placing 
    protected FullBunker _currentFullBunker;
    public FullBunker CurrentFullBunker
    {
        get // Only has get property as it shouldn't be set externally
        {
            if (CommonVariables.ManualBunkerPlacementActive) // Validates ManualBunkerPlacement is active
            {
                _currentFullBunker = FullBunkers[PlacementIteration]; // If so variable's returned
                return _currentFullBunker;
            }
            else // Otherwise errors output + null is returned
            {
                Debug.LogError("Bunker placing is no longer occuring but CurrentFullBunker requested.");
                return null;
            }
        }
    }
    // Variable to store reference to tile which player's currently hovering over. 
    private Tile _currentOnMouseOverTile; //Set by TileOnMouseOver (which is called by a tile when player mouses over it)

    private void Update() // Called every frame by unity
    {
        // MAINTANANCE - Could add more feedback for rotation - EG: Worded prompt + sound effect when rotated

        // Checks if R has been inputted + that manualbunkerplacement's active (
        if (Input.GetKeyUp(KeyCode.R) && CommonVariables.ManualBunkerPlacementActive)
        {
            // If so it rotates the bunker + updates tiles colours
            FullBunker fullBunker = FullBunkers[PlacementIteration];
            UpdateFullBunkerTilesColour(null, fullBunker, _currentOnMouseOverTile.Row, _currentOnMouseOverTile.Col, true); // Updates full bunkers tiles to default colour (true input to signify temporary)
            BunkerGenerator.RotateBunker(fullBunker); // Rotates the bunker by calling RotateBunker
        }
    }

    override public void TileOnMouseOver(Tile tile)
    {
        _currentOnMouseOverTile = tile;

        // MAINTANANCE - Could add tile interaction sound effect for usability feature for the deaf/visually impaired

        // If manual bunker placement's currently active:
        if (CommonVariables.ManualBunkerPlacementActive)
        {

            // And the currently being placed bunker overlaps:
            if (BunkerGenerator.DoesBunkerOverlap(tile.Row, tile.Col, FullBunkers[PlacementIteration], this))
            {
                UpdateFullBunkerTilesColour(new Color(1, 0.3f, 0.275f, 0.9f), FullBunkers[PlacementIteration], tile.Row, tile.Col, true); // Current bunker (that doesn't fit) is visualised as red
            }
            else // If the currently being placed bunker doesn't overlap
            {
                UpdateFullBunkerTilesColour(new Color(0, 1f, 0.275f, 0.9f), FullBunkers[PlacementIteration], tile.Row, tile.Col, true); // Current bunker (that fits) is visualised as green
            }

        }
        else // If manual bunker placement's not active:
        {
            Color tempColor = tile.TileColour;  // Gets the sprites current colour
            tempColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
            tile.TileSpriteRenderer.color = tempColor; // Sets the sprites colour to the more transparent colour
        }
    }
}
