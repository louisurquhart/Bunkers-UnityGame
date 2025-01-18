using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTile : Tile // SubClass of Tile class with on clicked method specialised for player tiles
{
    [SerializeField] Sprite _playerTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _playerTileMissSprite; // reference to the sprite a grass tile will change to when hit
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
            playerGridManager.UpdateFullBunkerTilesColour(GridManager._defaultColour, playerGridManager.CurrentFullBunker, Row, Col, true);
        }
    }

    protected void OnMouseDown()
    {
        if (CommonVariables.ManualBunkerPlacementActive &&  GridManager is PlayerGridManager playerGridManager && !BunkerGenerator.DoesBunkerOverlap(Row, Col, playerGridManager.CurrentFullBunker, GridManager))
        {
            Debug.Log("OnMouseDown: Calling ManualBunkerGenerator ");
            BunkerGenerator.ManualBunkerGenerator(Row, Col, playerGridManager);
        }
        else
        {
            Debug.Log("OnMouseDown: Player clicked their board when bunker placement not active/bunker overlaps ");
        }
    }
}
