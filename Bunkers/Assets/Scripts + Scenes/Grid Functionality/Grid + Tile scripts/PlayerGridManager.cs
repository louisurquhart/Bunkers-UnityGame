using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridManager : GridManager
{
    // Extra logic/variables added to GridManager for the players manual bunker generation options
    public int placementIteration;

    protected FullBunker currentFullBunker;
    public FullBunker CurrentFullBunker
    {
        get
        {
            if (CommonVariables.ManualBunkerPlacementActive)
            { currentFullBunker = FullBunkers[placementIteration]; return currentFullBunker; }
            else { Debug.LogError("Bunker placing is no longer occuring but CurrentFullBunker requested."); return null; }
        }
    }
    private Tile _currentOnMouseOverTile;

    private void Update() // Called every frame by unity
    {
        // Checks if R has been inputted + that manualbunkerplacement's active (
        if (Input.GetKeyUp(KeyCode.R) && CommonVariables.ManualBunkerPlacementActive)
        {
            // If so it rotates the bunker + updates tiles colours
            FullBunker fullBunker = FullBunkers[placementIteration];
            Debug.Log($"Updating fullbunkers colour to default (maybe): fullBunker: {fullBunker}, baserow: {fullBunker.BaseRow}, basecol: {fullBunker.BaseCol}");
            UpdateFullBunkerTilesColour(null, fullBunker, _currentOnMouseOverTile.Row, _currentOnMouseOverTile.Col, true);
            BunkerGenerator.RotateBunker(fullBunker);
        }
    }

    override public void TileOnMouseOver(Tile tile)
    {
        _currentOnMouseOverTile = tile;
        // If manual bunker placement's currently active:
        if (CommonVariables.ManualBunkerPlacementActive) 
        {
            
            // And the currently being placed bunker overlaps:
            if (BunkerGenerator.DoesBunkerOverlap(tile.Row, tile.Col, FullBunkers[placementIteration], this))
            {
                UpdateFullBunkerTilesColour(new Color(1, 0.3f, 0.275f, 0.9f), FullBunkers[placementIteration], tile.Row, tile.Col, true); // Current bunker (that doesn't fit) is visualised as red
            }
            else // If the currently being placed bunker doesn't overlap
            {
                UpdateFullBunkerTilesColour(new Color(0, 1f, 0.275f, 0.9f), FullBunkers[placementIteration], tile.Row, tile.Col, true); // Current bunker (that fits) is visualised as green
            }
            
        }
        // If manual bunker placement's not active:
        else
        { 
            Color tempColor = tile.TileColour;  // Gets the sprites current colour
            tempColor.a = 0.5f; // Changes the alpha of the sprites colour colour to 125 (more transparent)
            tile.TileSpriteRenderer.color = tempColor;
        }
    }
}
