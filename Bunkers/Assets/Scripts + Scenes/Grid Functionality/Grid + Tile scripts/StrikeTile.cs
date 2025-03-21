using UnityEngine;

// Subclass of tile with specialised proceudres for strike grid tiles
public class StrikeTile : Tile
{
    // Automatically called by unity if tile's clicked
    protected void OnMouseDown()
    {
        if (CommonVariables.ManualBunkerPlacementActive) // If manual bunker generation's active
        {
            // A log is output to signify no action's taken
            Debug.Log("Player hit enemy board when manual bunker placement's active. No action performed");
        }
        else if (PlayerPrefs.GetInt("SpecialStrikeStatus", 0) == 0 && GridManager.SpecialStrikeFunctionality is PlayerSpecialStrikeFunctionality playerSpecialStrikeFunctionality) // If special strikes are enabled (0 == enabled) + safely casts specialstrikefunctionality to playerspecialstrikefunctionality
        {
            // A log for testing's output + a special weapon's activated.
            Debug.Log($"SpecialStrikesEnabled (Status: {PlayerPrefs.GetInt("SpecialStrikeStatus", 0)}). Activating special weapon");
            GridManager.SpecialStrikeFunctionality.UseSpecialWeapon(null, Row, Col, GridManager); // Null is input to signify no weapon has been chosen as it's using selected weapon
        }
        else // Otherwise if special strikes aren't enabled + manual bunker generation's not active
        {
            // A normal strike's performed and outputs a log for testing
            Debug.Log($"SpecialStrikes not enabled (PlayerPrefs Status: {PlayerPrefs.GetInt("SpecialStrikeStatus", 0)}, SpecialStrikeFunctionality: {GridManager.SpecialStrikeFunctionality}). Performing regular strike");
            _gridManager.OnTileHit(this, true);
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
        GridManager.TileOnMouseOver(this); // Calls the gridManager's tileOnMouseOver method
    }

    protected void OnMouseExit() // Automatically called if tile's no longer being hovered over
    {
        TileSpriteRenderer.color = TileColour; // Sets the tile's colour back to its original colour
    }
}

