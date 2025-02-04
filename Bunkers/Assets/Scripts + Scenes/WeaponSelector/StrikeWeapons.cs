using UnityEngine;

// Base class for a special strike weapon which all sub classes use/modify procedures and variables from
public abstract class SpecialStrikeWeapon : ScriptableObject
{ 
    protected GameObject _weaponButton;
    public GameObject WeaponButton // Property for the in game button weapon selector the button inhibits (if it's for the player). Set in PlayerSpecialStrikeFunctionality.
    {
        get { return _weaponButton; }
        set { _weaponButton = value; }
    }

    abstract public string PlayerPrefName // Property for playerpref name with a get only accessor (no set as it's set as a constant inside the derived methods)
    {
        get;
    }

    public int TotalUsesLeft; // Int property for the total uses left of the weapon
    abstract public void Activate(int row, int col, GridManager gridManager); // Abstract procedure which needs to be overriden by all subprocedures as it executes the unique weapon functionality

    protected static void validateAndHitTilePosition(int row, int col, GridManager gridManager) // Procedure to validate a tile's within bounds of the Grid array before hitting it (used by strikes which use positions which are relative to another tiles position)
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

    protected static void hitAdjacentTiles(int row, int col, GridManager gridManager) // Procedure to hit adjacent tiles to a designated position
    {
        // Hits all adjacent tiles (via validateAndHitTilePosition method as they could be out of bounds)
        validateAndHitTilePosition(row + 1, col, gridManager);
        validateAndHitTilePosition(row - 1, col, gridManager);
        validateAndHitTilePosition(row, col + 1, gridManager);
        validateAndHitTilePosition(row, col - 1, gridManager);
    }
}

// Subclass inherited from special strike weapon - Default strike only hits one tile 
public class DefaultStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName // Overriden playerprefname property
    {
        get { Debug.LogWarning("DefaultStrike: playerprefname attempted to be used, should't be possible. Null returned."); return null;} // Outputs warning + returns null as it should never need to be accessed
                                                                                                                                          // (as its uses is unlimited so can't be modified so no value needs to be loaded)
    }
    override public void Activate(int row, int col, GridManager gridManager) // Overriden activate procedure
    {
        if (TotalUsesLeft > 0)
        {
            gridManager.OnTileHit(gridManager.Grid[row, col], true); // Hits one tile 
        }
        else // If it has no uses left
        {
            Debug.LogError($"DefaultStrike called with no uses left (totalUsesLeft = {TotalUsesLeft})"); // Outptus error as this should never be possible (uses are unlimited)
        }
    }
}

// Subclass inherited from special strike weapon - Random strike hits 3 random tiles
public class RandomStrike : SpecialStrikeWeapon
{
    public override string PlayerPrefName
    {
        get { return "RandomStrikeQuantity"; } // Returns a constant predefined playerpref value
    }
    override public void Activate(int row, int col, GridManager gridManager) // Overriden activate procedure
    {
        gridManager.OnTileHit(gridManager.Grid[row, col], false); // Hits the tile clicked 

        // + 2 random tiles
        for (int i = 0; i < 2; i++)
        {
            gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager), false); // Generates random tile and hits it (position already valid so no need for extra validation)
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
        get { return "QuadStrikeQuantity"; } // Returns a constant predefined playerpref value
    }
    override public void Activate(int row, int col, GridManager gridManager) // Overriden activate procedure
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
        get { return "OctaStrikeQuantity"; } // Returns a constant predefined playerpref value
    }
    override public void Activate(int row, int col, GridManager gridManager) // Overriden activate procedure
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
