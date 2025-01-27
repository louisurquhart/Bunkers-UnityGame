using UnityEngine;

public class PlayerTile : Tile // SubClass of Tile class with on clicked method specialised for player tiles
{
    // GridManager reference
    override public GridManager GridManager
    {
        get { return gridManager; }
    }

    public override void SetBunker(FullBunker givenBunkerType) // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    {
        FullBunkerReference = givenBunkerType;
        IsBunker = true; // Sets IsBunker to true to designate the tile as a bunker tile
        UpdateTileColour(FullBunkerReference.BunkerColor, false); // Changes the colour of the tile to whatever the colour of the whole bunker is
    }

    // --- Procedures to highlight tile when it's hovered over ---
    protected void OnMouseOver() // Automatically called by unity if tile's hovered over. Temporaraly makes the tile more transparent
    {
        GridManager.TileOnMouseOver(this);
    }

    protected void OnMouseExit() // Automatically called if tile's no longer being hovered over. Sets the tile back to its regular colour
    {
        TileSpriteRenderer.color = TileColour;

        if (CommonVariables.ManualBunkerPlacementActive && GridManager is PlayerGridManager playerGridManager)
        { 
            playerGridManager.UpdateFullBunkerTilesColour(GridManager.DefaultColour, playerGridManager.CurrentFullBunker, Row, Col, true);
        }
    }

    protected void OnMouseDown()
    {
        // If manual bunker placement's active + position is valid for bunker placement
        if (CommonVariables.ManualBunkerPlacementActive && GridManager is PlayerGridManager playerGridManager && !BunkerGenerator.DoesBunkerOverlap(Row, Col, playerGridManager.CurrentFullBunker, GridManager))
        {
            // A bunker's placed
            BunkerGenerator.ManualBunkerGenerator(Row, Col, playerGridManager);
        }
        else // Otherwise a debug log is output to signify what occured
        {
            Debug.Log("OnMouseDown: Player clicked their board when bunker placement not active/bunker overlaps ");
        }
    }
}
