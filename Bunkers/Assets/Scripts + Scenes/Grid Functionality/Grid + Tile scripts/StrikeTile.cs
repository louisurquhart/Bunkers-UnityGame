using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subclass of tile with specialised proceudres for strike grid tiles
public class StrikeTile : Tile 
{
    [SerializeField] Sprite _aiTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _aiTileMissSprite; // reference to the sprite a grass tile will change to when hit

    // Automatically called by unity if tile's clicked
    protected void OnMouseDown() 
    {
        GridManager.OnTileHit(this); // Calls OnTileClicked function as tile has been clicked
    }

    // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    public override void SetBunker(FullBunker givenBunkerType)
    {
        FullBunkerReference = givenBunkerType; // Sets FullBunkerReference to the given fullBunker
        IsBunker = true; 
    }
}

