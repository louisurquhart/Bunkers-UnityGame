using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeTile : Tile // Subclass of tile with OnTileClicked method specialised tiles for strike grid tiles
{
    [SerializeField] Sprite _aiTileHitSprite; // reference to the sprite a bunker tile will change to when hit
    [SerializeField] Sprite _aiTileMissSprite; // reference to the sprite a grass tile will change to when hit

    public override bool OnTileClicked() // Override method of OnTileHit function. Does immediate validation of hit
    {
        if (!isPreviouslyHit && !CommonVariables.PlayerTurn) // makes sure the tile hasn't been hit before + validating it's definitely the AI's turn
        {
            Debug.Log($"{TileOwner} owned tile hit at: Row = {row} Column = {col}"); // outputs into the unity console that a tile has been detected to have been clicked. (for  testing purposes)

            isPreviouslyHit = true; // sets the isHit variable to true to signifiy it has been clicked (incase it's clicked again)

            if (IsBunker == true) // if tile's a bunker 
            {
                _gridManager.OnBunkerHit(row, col); // it calls the onhit function in the gridmanager script inputting its row and column as variables
                return true; // returns true to signify that the method completed successfully
            }
            else // if tile's not a bunker 
            {
                _gridManager.OnBunkerMiss(row, col); // it calls the onmiss function in the gridmanager script inputting its row and column as variables
                return true; // returns true to signify that the method completed successfully
            }
        }
        else if (isPreviouslyHit)
        {
            Debug.Log("OnTileClicked (StrikeTile): Player tried to hit a tile which has already been hit"); // outputs error that that validation blocked the player trying to hit a tile when it's not their turn
            return false; // returns false to signify that the method failed to complete successfully due to one or more validation failiures
        }
        else
        {
            Debug.Log("OnTileClicked (StrikeTile): Player tried to hit a tile when it's not their turn"); // outputs info message that validation blocked the player from clicking a tile when it's not their turn for debugging + testing
            return false; // returns false to signify that the method failed to complete successfully due to one or more validation failiures
        }
    }


    public override void SetBunker(int bunkerNumber, Bunker bunkerType) // Procedure to be called when a tile is designated as a bunker (when bunker generation is happening)
    {
        IsBunker = true;
    }

}

