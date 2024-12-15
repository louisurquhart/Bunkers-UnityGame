using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeGridManager : GridManager // Specialised version of GridManager for the strike grid (AI's) with modified methods
{
    public override void OnBunkerHit(int row, int col) // function called when a tile with a bunkers been hit (validation already done)
    {
        Debug.Log("StrikeGridManager: Determined tile was a bunker"); // outputs into the unity console that it's identified cell has been clicked. (for debugging + testing purposes)

        Tile clickedTile = Grid[row, col]; // finds the copy of the tile class which has been clicked through the array referencing its row + column

        // In future will need to check if the tile's a special bunker here (special bunkers will be implemented in final prototype)

        clickedTile.TileSpriteRenderer.color = Color.red; // changes the tiles colour to red (will be a sprite in final iteration just for prototype & testing)

        // Hit animation will be added in the final iteration
        // Sound effect will be added in final iteration

        GeneralBackgroundLogic.ChangeTurn();

        // Update statistics when they're implemented (enemy bunkers hit + maybe special bunker hit too)

        GeneralBackgroundLogic.HasGameEnded(); // Calls HasGameEnded function to check if this hit was gameending. It will deal with all outcomes
    }
    public override void OnBunkerMiss(int row, int col)
    {
        Debug.Log("StrikeGridManager: Determined tile wasn't a bunker"); // outputs into the unity console that it's identified cell has been clicked. (for debugging + testing purposes)

        Tile clickedTile = Grid[row, col]; // finds the copy of the tile class which has been clicked through the array referencing its row + column

        // In future will need to check if the tile's a special bunker here (special bunkers will be implemented in final prototype)

        clickedTile.TileSpriteRenderer.color = Color.blue; // changes the tiles colour to blue to indicate miss (will be a sprite in final iteration just for prototype & testing)

        // Hit animation will be added in the final iteration
        // Sound effect will be added in final iteration

        GeneralBackgroundLogic.ChangeTurn();

        // Update statistics when they're implemented (enemy bunkers hit + maybe special bunker hit too)


    }
}