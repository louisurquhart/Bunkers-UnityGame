using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.Rendering.DebugUI.Table;

public class SpecialStrikeFunctionality : MonoBehaviour
{
    //Array to store weapon sub classes
    public SpecialStrikeWeapon[] Weapons = new SpecialStrikeWeapon[4];

    // Reference to current active weapon
    protected SpecialStrikeWeapon currentWeapon;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("SpecialStrikeStatus", 0) != 0) // If special strikes aren't enabled by the player (0 == enabled, 1 == disabled)
        {
            // The special strike class is destroyed
            Debug.Log($"<b>SpecialStrikeFunctionality SpecialStrikes not enabled (SpecialStrikeStatus: {PlayerPrefs.GetInt("SpecialStrikeStatus")}. Self destroying");
            GameObject.Destroy(this);
        }
        // Weapon sub classes are instantiated and added to the array 
        Weapons[0] = ScriptableObject.CreateInstance<DefaultStrike>();
        Weapons[1] = ScriptableObject.CreateInstance<RandomStrike>();
        Weapons[2] = ScriptableObject.CreateInstance<QuadrupleStrike>();
        Weapons[3] = ScriptableObject.CreateInstance<OctaStrike>();

        currentWeapon = Weapons[0];
    }



    // Procedure to use a weapon
    virtual public void UseSpecialWeapon(SpecialStrikeWeapon givenWeapon, int row, int col, GridManager gridManager)
    {
        if (givenWeapon == null) // If no special weapon's given, a default strike's performed instead
        {
            gridManager.OnTileHit(gridManager.Grid[row, col], true);
            Debug.LogWarning("UseSpecialWeapon called but special weapon == null. Default strike peformed"); // Warnings output to console 
        }
        else // Otherwise if a special weapon's given, a special strike's performed
        {
            if (givenWeapon.TotalUsesLeft > 0) // If the weapon has uses left (validation)
            {
                givenWeapon.Activate(row, col, gridManager); // The weapon's activated at the given row + column
                givenWeapon.TotalUsesLeft--; // Total uses left of the weapon's decremented
            }
            else // If weapon has been used with no uses left (shouldn't be possible, validation failiure)
            {
                Debug.LogError("Weapon has no uses left. No action performed (should be impossible)"); // An error is outputted for debugging
            }
        }
    }
}




