using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialStrikeFunctionality : SpecialStrikeFunctionality
{
    // Extra player specific code  (All for visual weapon selector functionality)

    // Color stucts created of the inactive + active weapon colours
    Color _inactiveWeaponColour = new Color(1, 1, 1, 0.75f);
    Color _activeWeaponColour = new Color(1, 1, 1, 1);

    // Weapon buttons array to hold references to the buttons for each weapon (set in unity inspector)
    [SerializeField] GameObject[] WeaponButtons = new GameObject[4];

    void Start() // Called by unity after instantiation
    {
        // Goes through each weapon 
        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].WeaponButton = WeaponButtons[i]; // Sets the weapons button to the corresponding weapon button in the array
        }
        
        LoadSavedWeaponsUses(); // Loads the weapon status (uses + activeness) onto the players buttons
    }

    // Procedure to load saved weapon uses onto the weapon selector buttons
    private void LoadSavedWeaponsUses()
    {
        // Sets all special weapons uses left to their saved playerpref values. If there's no saved values a default value of 1 is set
        for(int i = 1; i < Weapons.Length; i++)
        {
            SpecialStrikeWeapon weapon = Weapons[i]; // Finds the current special weapon

            weapon.TotalUsesLeft = PlayerPrefs.GetInt(weapon.PlayerPrefName, 1); // Updates total uses left for the weapon with the saved value
            UpdateWeaponStatus(weapon); // Updates the weapons visual status
        }

        // Sets default weapon to 81 as it has a infinite amount of uses (81 is total amount of grid squares)
        Weapons[0].TotalUsesLeft = 81;
    }

    // Procedure to use a weapon (overriden from base class)
    override public void UseSpecialWeapon(SpecialStrikeWeapon givenWeapon, int row, int col, GridManager gridManager)
    {
        if (_currentWeapon.TotalUsesLeft > 0) // If the weapon has uses left (validation)
        {
            _currentWeapon.Activate(row, col, gridManager); // The weapon's activated at the given row + column

            _currentWeapon.TotalUsesLeft--; // Total uses left of the weapon's decremented
            UpdateWeaponStatus(_currentWeapon); // The weapons status is updated
        }
        else // If weapon has been used with no uses left (shouldn't be possible, validation failiure)
        {
            Debug.LogError("Weapon has no uses left. No action performed (should be impossible)"); // An error is outputted
        }
    }

    // Procedure to load the weapons uses left + status onto the players buttons

    // Prcoedure to change the current weapon. It's called by a weapon button (via unity inspector) with the weapons index passed in.
    public void ChangeSelectedWeapon(int weaponIntRef)
    {
        // Finds which weapon's being referenced
        SpecialStrikeWeapon newWeapon = Weapons[weaponIntRef];

        if (newWeapon.TotalUsesLeft > 0) // If weapon has uses left it's switched to
        {
            // Disables the outline for the old weapon showing it's no longer selected
            Outline currentWeaponOutlineComponent = _currentWeapon.WeaponButton.GetComponent<Outline>();
            currentWeaponOutlineComponent.enabled = false;

            // Enables the outline for the new weapon showing it's selected
            Outline newWeaponOutlineComponent = newWeapon.WeaponButton.GetComponent<Outline>();
            newWeaponOutlineComponent.enabled = true;

            // Sets current weapon to the new weapon
            _currentWeapon = newWeapon;
        }
        else // Otherwise if it has no uses left
        {
            Debug.Log("Weapon has no uses left. No action performed"); // Log output for testing
        }
    }


    // Procedure to update a weapons weapon selector status 
    private void UpdateWeaponStatus(SpecialStrikeWeapon givenWeapon)
    {
        if (givenWeapon.TotalUsesLeft <= 0) // If the weapon has 0 uses:
        {
            // Changes transparency/colour of weapon to more transparent to show it's inactive
            givenWeapon.WeaponButton.GetComponent<Image>().color = _inactiveWeaponColour;
            // Changes current weapon back to default as this weapon no longer has any uses left
            ChangeSelectedWeapon(0);
        }
        else // If the weapon has uses
        {
            // Changes transparency/colour of weapon to more transparent to show it's inactive
            givenWeapon.WeaponButton.GetComponent<Image>().color = _activeWeaponColour;
        }

        // Updates uses left text on the buttons (if special, if not it's default which has no use counter)
        if (givenWeapon is not DefaultStrike)
        {
            givenWeapon.WeaponButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = givenWeapon.TotalUsesLeft.ToString();
        }
    }
}
