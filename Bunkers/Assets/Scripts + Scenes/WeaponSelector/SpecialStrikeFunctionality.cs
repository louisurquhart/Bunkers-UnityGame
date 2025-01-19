using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.Rendering.DebugUI.Table;

public class SpecialStrikeFunctionality : MonoBehaviour
{
    // Array to store weapon sub classes
    //SpecialStrikeWeapon[] weapons = new SpecialStrikeWeapon[4]
    //{
    //    // Weapon sub classes are instantiated and added to the array 
    //    new DefaultStrike(),
    //    new RandomStrike(),
    //    new QuadrupleStrike(),
    //    new OctaStrike(),
    //};

    // Reference to current active weapon
    SpecialStrikeWeapon currentWeapon;

    // ChangeWeapon is called by a weapon button (via unity inspector) to change the players current weapon
    public void ChangeWeapon(SpecialStrikeWeapon newWeapon)
    {
        if (newWeapon.TotalUsesLeft > 0)
        {
            // Disables the outline for the old weapon showing it's no longer selected
            Outline currentWeaponOutlineComponent = currentWeapon.WeaponButton.GetComponent<Outline>();
            currentWeaponOutlineComponent.enabled = false;

            // Enables the outline for the new weapon showing it's selected
            Outline newWeaponOutlineComponent = newWeapon.GetComponent<Outline>();
            newWeaponOutlineComponent.enabled = true;
        }
        else
        {
            Debug.Log("Weapon has no uses left. No action performed");
        }

    }
    public void UseWeapon(SpecialStrikeWeapon weapon)
    {

    }
}

public abstract class SpecialStrikeWeapon : MonoBehaviour
{
    public GameObject WeaponButton;
    public int TotalUsesLeft;

    public void Instantiate(GameObject givenWeaponButton, int givenTotalUsesLeft)
    {
       TotalUsesLeft = givenTotalUsesLeft;
       WeaponButton = givenWeaponButton;
    }
    abstract public void Activate(int row, int col, GridManager gridManager);
    protected static void validateAndHitTilePosition(int row, int col, GridManager gridManager)
    {
        // Validates that position is within bounds of the grid
        if (row >= 0 && row < gridManager.Grid.GetLength(0) && col >= 0 && col < gridManager.Grid.GetLength(1))
        { return; } // If not it exits performing nothing

        // If it is within bounds it hits the tile
        else { gridManager.OnTileHit(gridManager.Grid[row, col]); }
    }
    protected static void hitAdjacentTiles(int row, int col, GridManager gridManager)
    {
        validateAndHitTilePosition(row + 1, col, gridManager);
        validateAndHitTilePosition(row - 1, col, gridManager);
        validateAndHitTilePosition(row, col + 1, gridManager);
        validateAndHitTilePosition(row, col - 1, gridManager);
    }
}

public class DefaultStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager));
    }
}
public class RandomStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        for (int i = 0; i < 3; i++)
        {
            gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager)); // Generates random tile and hits it (position already valid so no need for extra validation
        }
    }
}

public class QuadrupleStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        // Hits the tile itself (already validated)
        gridManager.OnTileHit(gridManager.Grid[row, col]);

        // Hits the 4 adjacent tile's to the tile (validates before hitting as they could be off-grid)
        hitAdjacentTiles(row, col, gridManager);
    }
}
public class OctaStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        // Hits the tile itself (already validated)
        gridManager.OnTileHit(gridManager.Grid[row, col]);

        // Hits the 4 adjacent tile's to the tile (validates before hitting as they could be off-grid)
        hitAdjacentTiles(row, col, gridManager);

        // Hits the 4 diagonal tiles to the tile (validates before hitting as they could be off-grid)
        gridManager.OnTileHit(gridManager.Grid[row + 1, col + 1]);
        gridManager.OnTileHit(gridManager.Grid[row + 1, col - 1]);
        gridManager.OnTileHit(gridManager.Grid[row - 1, col + 1]);
        gridManager.OnTileHit(gridManager.Grid[row - 1, col - 1]);
    }
}

