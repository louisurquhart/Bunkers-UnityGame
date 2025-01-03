using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTile : Tile // SubClass of Tile class with on clicked method specialised for player tiles
{
    [SerializeField] Sprite _playerTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _playerTileMissSprite; // reference to the sprite a grass tile will change to when hit

    public override void SetBunker(FullBunker givenBunkerType) // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    {
        FullBunkerReference = givenBunkerType;
        IsBunker = true; // Sets IsBunker to true to designate the tile as a bunker tile
        UpdateTileColour(FullBunkerReference.BunkerColor); // Changes the colour of the tile to whatever the colour of the whole bunker is
    }
}
