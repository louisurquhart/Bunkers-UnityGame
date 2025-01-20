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


    virtual public void UseWeapon(SpecialStrikeWeapon givenWeapon, int row, int col)
    {
        if(givenWeapon.TotalUsesLeft > 0) // If the weapon has uses left (validation)
        {
            givenWeapon.Activate(row, col); // The weapon's activated at the given row + column
            weapon.TotalUsesLeft--; // Total uses left of the weapon's decremented
        }
        else // If weapon has been used with no uses left (shouldn't be possible, validation failiure)
        {
            Debug.LogError("Weapon has no uses left. No action performed") // An error is outputted for debugging
        }
    }

    // ----------------------------------------------------------------------------------------- //
    // PLAYER SPECIFIC CODE BELOW FOR THEIR WEAPON SELECTOR - NEEDS MOVING TO POLYMORBISM SUBCLASS:

    // ChangeWeapon is called by a weapon button (via unity inspector) to change the players current weapon
    void Start()
    {
        // Loads the weapon status (uses + activeness) onto the players buttons
        LoadSavedWeaponUses();
    }

    //override public void UseWeapon(SpecialStrikeWeapon givenWeapon, int row, int col)
    //{
    //    if (givenWeapon.TotalUsesLeft > 0) // If the weapon has uses left (validation)
    //    {
    //        givenWeapon.Activate(row, col); // The weapon's activated at the given row + column

    //        weapon.TotalUsesLeft--; // Total uses left of the weapon's decremented
    //        UpdateWeaponStatus(givenWeapon) // The weapons status is updated
    //    }
    //    else // If weapon has been used with no uses left (shouldn't be possible, validation failiure)
    //    {
    //        Debug.LogError("Weapon has no uses left. No action performed") // An error is outputted
    //    }
    //}

    // Procedure to load the weapons uses left + status onto the players buttons
    public void LoadSavedWeaponUses()
    {
        // Sets different special strikes uses left to their saved values. If there's no saved values a default value of 1 is set
        RandomStrike.TotalUsesLeft = PlayerPrefs.GetInt("TotalRandomStrikes", 1);
        QuadrupleStrike.TotalUsesLeft = PlayerPrefs.GetInt("TotalQuadrupleStrikes", 1);
        OctaStrike.TotalUsesLeft = PlayerPrefs.GetInt("TotalOctaStrikes", 1);

        // The status of these weapons is then updated
        UpdateWeaponStatus(RandomStrike);
        UpdateWeaponStatus(QuadrupleStrike);
        UpdateWeaponStatus(OctaStrike);
    }

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

    public void UpdateWeaponStatus(SpecialStrikeWeapon givenWeapon)
    {
        if(givenWeapon.TotalUsesLeft <= 0) // If the weapon no longer has any uses left:
        {
            // needs to change transparency of weapon to more transparent to show it's inactive
            //givenWeapon.WeaponButton.Color;

            // Changes current weapon back to default as this weapon no longer has any uses left
            ChangeWeapon(DefaultStrike);
        }
        else // If the weapon has uses left
        {
            // Change weapon transparency to normal
        }

        // Updates uses left text on the buttons:
        //givenWeapon.WeaponButton.text = givenWeapon.TotalUsesLeft; // probably something like this
    }
}

protected abstract class SpecialStrikeWeapon : MonoBehaviour
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

internal class DefaultStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager));
    }
}
internal class RandomStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        for (int i = 0; i < 3; i++)
        {
            gridManager.OnTileHit(GeneralBackgroundLogic.GenerateRandomTile(gridManager)); // Generates random tile and hits it (position already valid so no need for extra validation
        }
    }
}

internal class QuadrupleStrike : SpecialStrikeWeapon
{
    override public void Activate(int row, int col, GridManager gridManager)
    {
        // Hits the tile itself (already validated)
        gridManager.OnTileHit(gridManager.Grid[row, col]);

        // Hits the 4 adjacent tile's to the tile (validates before hitting as they could be off-grid)
        hitAdjacentTiles(row, col, gridManager);
    }
}
internal class OctaStrike : SpecialStrikeWeapon
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

