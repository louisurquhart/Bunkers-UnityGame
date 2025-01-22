using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialStrikeFunctionality : SpecialStrikeFunctionality
{
    // Extra player specific code  (All for visual weapon selector)

    // Color stucts created of the inactive + active weapon colours
    Color inactiveWeaponColour = new Color(1, 1, 1, 0.75f);
    Color activeWeaponColour = new Color(1, 1, 1, 1);

    [SerializeField] GameObject[] WeaponButtons = new GameObject[4];
    void Start()
    {
        for(int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].WeaponButton = WeaponButtons[i];
            Debug.Log($"Iteration {i}: Weapon: {Weapons[i]} WeaponButton: {WeaponButtons[i]}");
        }
        
        LoadSavedWeaponsUses(); // Loads the weapon status (uses + activeness) onto the players buttons
    }

    private void LoadSavedWeaponsUses()
    {
        PlayerPrefs.SetInt("TotalRandomStrikes", 2);
        PlayerPrefs.SetInt("TotalQuadrupleStrikes", 2);
        PlayerPrefs.SetInt("TotalOctaStrikes", 2);
        // Sets different special strikes uses left to their saved values. If there's no saved values a default value of 1 is set
        Weapons[1].TotalUsesLeft = PlayerPrefs.GetInt("TotalRandomStrikes", 1);
        Weapons[2].TotalUsesLeft = PlayerPrefs.GetInt("TotalQuadrupleStrikes", 1);
        Weapons[3].TotalUsesLeft = PlayerPrefs.GetInt("TotalOctaStrikes", 1);
        Debug.Log("WeaponUsesUpdated");

        // The status of these special weapons is then updated
        Weapons[0].TotalUsesLeft = 81;
        UpdateWeaponStatus(Weapons[1]);
        UpdateWeaponStatus(Weapons[2]);
        UpdateWeaponStatus(Weapons[3]);
    }


    override public void UseSpecialWeapon(SpecialStrikeWeapon givenWeapon, int row, int col, GridManager gridManager)
    {
        if (currentWeapon.TotalUsesLeft > 0) // If the weapon has uses left (validation)
        {
            currentWeapon.Activate(row, col, gridManager); // The weapon's activated at the given row + column

            currentWeapon.TotalUsesLeft--; // Total uses left of the weapon's decremented
            UpdateWeaponStatus(currentWeapon); // The weapons status is updated
        }
        else // If weapon has been used with no uses left (shouldn't be possible, validation failiure)
        {
            Debug.LogError("Weapon has no uses left. No action performed (should be impossible)");// An error is outputted
        }
    }

    // Procedure to load the weapons uses left + status onto the players buttons

    // ChangeWeapon is called by a weapon button (via unity inspector) to change the players current weapon
    public void ChangeSelectedWeapon(int weaponIntRef)
    {
        // Finds which weapon's being referenced
        SpecialStrikeWeapon newWeapon = Weapons[weaponIntRef];

        if (newWeapon.TotalUsesLeft > 0) // If weapon has uses left it's switched to
        {
            // Disables the outline for the old weapon showing it's no longer selected
            Outline currentWeaponOutlineComponent = currentWeapon.WeaponButton.GetComponent<Outline>();
            currentWeaponOutlineComponent.enabled = false;

            // Enables the outline for the new weapon showing it's selected
            Outline newWeaponOutlineComponent = newWeapon.WeaponButton.GetComponent<Outline>();
            newWeaponOutlineComponent.enabled = true;

            currentWeapon = newWeapon;
            Debug.Log($"Current weapon changed to {newWeapon}");
        }
        else // Otherwise if it has no uses left
        {
            Debug.Log("Weapon has no uses left. No action performed");
        }
    }


    // Procedure to update a weapons weapon selector status 
    private void UpdateWeaponStatus(SpecialStrikeWeapon givenWeapon)
    {
        Debug.Log($"Updating {givenWeapon}. Previous total uses left: {givenWeapon.TotalUsesLeft}");

        if (givenWeapon.TotalUsesLeft <= 0) // If the weapon has 0 uses:
        {
            // Changes transparency/colour of weapon to more transparent to show it's inactive
            givenWeapon.WeaponButton.GetComponent<Image>().color = inactiveWeaponColour;
            // Changes current weapon back to default as this weapon no longer has any uses left
            ChangeSelectedWeapon(0);
        }
        else // If the weapon has uses
        {
            // Changes transparency/colour of weapon to more transparent to show it's inactive
            givenWeapon.WeaponButton.GetComponent<Image>().color = activeWeaponColour;
        }

        // Updates uses left text on the buttons (if special)
        if (givenWeapon is not DefaultStrike)
        {
            givenWeapon.WeaponButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = givenWeapon.TotalUsesLeft.ToString();
        }
    }
}
