using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

// Subclass of tile with specialised proceudres for strike grid tiles
public class StrikeTile : Tile 
{
    [SerializeField] Sprite _aiTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _aiTileMissSprite; // reference to the sprite a grass tile will change to when hit

    // Automatically called by unity if tile's clicked
    protected void OnMouseDown() 
    {
        if (!CommonVariables.ManualBunkerPlacementActive)
        {
            GridManager.OnTileHit(this); // Calls OnTileClicked function as tile has been clicked
        }
    }

    // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    public override void SetBunker(FullBunker givenBunkerType)
    {
        FullBunkerReference = givenBunkerType; // Sets FullBunkerReference to the given fullBunker
        IsBunker = true;
    }

    // --- Procedures to highlight tile when it's hovered over ---
    protected void OnMouseOver() // Automatically called by unity if tile's hovered over. Temporaraly makes the tile more transparent
    {
        GridManager.TileOnMouseOver(this);
    }

    protected void OnMouseExit() // Automatically called if tile's no longer being hovered over. Sets the tile back to its regular colour
    {
        TileSpriteRenderer.color = TileColour;
    }
}

