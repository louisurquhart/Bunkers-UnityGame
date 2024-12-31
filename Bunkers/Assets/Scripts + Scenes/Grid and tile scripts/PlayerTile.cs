using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTile : Tile // SubClass of Tile class with on clicked method specialised for player tiles
{
    [SerializeField] Sprite _playerTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _playerTileMissSprite; // reference to the sprite a grass tile will change to when hit


    protected void OnMouseDown()
    {
        //OnTileClicked()
        //if (CommonVariables.ShipPlacingIsActive)
        //{
        //    // Call manual placement of ships
        //}
        //else
        //{
        //    Debug.Log("INFO - OnTileClicked: Player clicked on one of their tiles when ShipPlacingIsActive = {ShipPlacingIsActive");
        //}
    }

    public override void SetBunker(int bunkerNumber, Bunker bunkerType) // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    {
        IsBunker = true; // Sets IsBunker to true to designate the tile as a bunker tile
        TileSpriteRenderer.color = bunkerType.BunkerColor; // Changes the colour of the tile to whatever the colour of the whole bunker is
    }
}
