using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeTile : Tile // Subclass of tile with OnTileClicked method specialised tiles for strike grid tiles
{
    [SerializeField] Sprite _aiTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _aiTileMissSprite; // reference to the sprite a grass tile will change to when hit

    protected void OnMouseDown() // Automatically called by unity if tile's clicked
    {
        OnTileHit(); // Calls OnTileClicked function as tile has been clicked
    }


    public override void SetBunker(int bunkerNumber, Bunker bunkerType) // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    {
        IsBunker = true;
    }

}

