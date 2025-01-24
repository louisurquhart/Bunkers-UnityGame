
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.Rendering.DebugUI.Table;

// Base class for a special strike weapon which all sub classes use/modify procedures and variables from
public abstract class SpecialStrikeWeapon : ScriptableObject
{ 
    protected GameObject _weaponButton;
    public GameObject WeaponButton
    {
        get { return _weaponButton; }
        set { _weaponButton = value; }
    }

    abstract public string PlayerPrefName
    {
        get;
    }
    public int TotalUsesLeft;
    abstract public void Activate(int row, int col, GridManager gridManager);
    protected static void validateAndHitTilePosition(int row, int col, GridManager gridManager)
    {
        // Validates that position is within bounds of the grid
        if (!(row >= 0 && row < gridManager.Grid.GetLength(0) && col >= 0 && col < gridManager.Grid.GetLength(1)))
        { 
            Debug.Log($"Tile at row: {row} col: {col} out of bounds, no action performed. (Grid rows: {gridManager.Grid.GetLength(0)}, Grid columns: {gridManager.Grid.GetLength(1)})");
            Debug.Log($"Row validation {row >= 0 && row < gridManager.Grid.GetLength(0)} Column validation: {col >= 0 && col < gridManager.Grid.GetLength(1)}");
            return; 
        } // If not it exits performing nothing

        // If it is within bounds it hits the tile
        else { gridManager.OnTileHit(gridManager.Grid[row, col], false); }
    }
    protected static void hitAdjacentTiles(int row, int col, GridManager gridManager)
    {
        validateAndHitTilePosition(row + 1, col, gridManager);
        validateAndHitTilePosition(row - 1, col, gridManager);
        validateAndHitTilePosition(row, col + 1, gridManager);
        validateAndHitTilePosition(row, col - 1, gridManager);
    }
}



// Subclass inherited from special strike weapon - Default strike only hits one tile 
public class DefaultStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName
    {
        get { Debug.LogWarning("DefaultStrike: playerprefname attempted to be used, should't be possible. Null returned."); return null;  }
    }
    override public void Activate(int row, int col, GridManager gridManager)
    {
        if (TotalUsesLeft > 0)
        {
            gridManager.OnTileHit(gridManager.Grid[row, col], true);
        }
        else
        {
            Debug.LogError($"DefaultStrike called with no uses left (totalUsesLeft = {TotalUsesLeft} (s))");
        }
    }
}

// Subclass inherited from special strike weapon - Random strike hits 3 random tiles
public class RandomStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName
    {
        get { return "RandomStrikeQuantity"; }
    }
    override public void Activate(int row, int col, GridManager gridManager)
    {
        gridManager.OnTileHit(gridManager.Grid[row, col], false); // Hits the tile clicked 

        // + 2 random tiles
        for (int i = 0; i < 2; i++)
        {
            gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager), false); // Generates random tile and hits it (position already valid so no need for extra validation
        }
        
        // Turn's then changed
        GeneralBackgroundLogic.ChangeTurn();
    }
}

// Subclass inherited from special strike weapon - Quadruple strike hits the tile + 4 adjacent tiles to the desginated tile
public class QuadrupleStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName
    {
        get { return "QuadStrikeQuantity"; }
    }
    override public void Activate(int row, int col, GridManager gridManager)
    {
        // Hits the tile itself (already validated)
        gridManager.OnTileHit(gridManager.Grid[row, col], false);
        // Hits the 4 adjacent tile's to the tile (validates before hitting as they could be off-grid)
        hitAdjacentTiles(row, col, gridManager);
        GeneralBackgroundLogic.ChangeTurn();
    }
}

// Subclass inherited from special strike weapon - Octa strike hits the tile + 4 adjacent tiles + 4 diagonal tiles to the designated tile
public class OctaStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName
    {
        get { return "OctaStrikeQuantity"; }
    }
    override public void Activate(int row, int col, GridManager gridManager)
    {
        // Hits the tile itself (already validated)
        gridManager.OnTileHit(gridManager.Grid[row, col], false);

        // Hits the 4 adjacent tile's to the tile (validates before hitting as they could be off-grid)
        hitAdjacentTiles(row, col, gridManager);

        // Hits the 4 diagonal tiles to the tile (validates before hitting as they could be off-grid)
        validateAndHitTilePosition(row + 1, col + 1, gridManager);
        validateAndHitTilePosition(row + 1, col - 1, gridManager);
        validateAndHitTilePosition(row - 1, col + 1, gridManager);
        validateAndHitTilePosition(row - 1, col - 1, gridManager);
        GeneralBackgroundLogic.ChangeTurn();
    }
}
