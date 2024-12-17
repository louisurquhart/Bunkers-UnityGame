using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ConsoleFunctionality : MonoBehaviour
{
    [SerializeField] GameObject consolePanelUI; // reference to the console panel to toggle it on/off
    [SerializeField] GameObject consoleInputField; // reference to the consoles text input field
    bool consoleEnabled;

    Dictionary<string, Action> commands = new Dictionary<string, Action>(1); // Dictionary to convert string commands to their methods


    //private void Awake()
    //{
    //    // Console commands added in awake method as they use an instance of a class
    //    //commands.Add( "GenerateBunkers", ConsoleCommands.Instance.RandomlyGenerateBunkers); // Adds GenerateBunker command + the corrosponding method
    //}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tilde)) // If Tilde key is inputted 
        {
            ToggleConsole(); // The toggle console method is called to turn the console on/off
        }
    }

    private void ToggleConsole() // Method to enable/disable console 
    {
        consoleEnabled = !consoleEnabled; // Changes the consoleEnabled to whatever it isn't (toggles)
        consolePanelUI.SetActive(consoleEnabled); // Sets the in game console UI to enabled/disabled depending on what consoleEnabled is set to
    }

    public void InputCommand(string givencommand) // Method to validate and then execute a given command
    {
        if (commands.ContainsKey(givencommand)) // Checks if the inputted string is a command within the dictionary
        {
            Action commandMethod = commands[givencommand]; // Finds what method the key references
            commandMethod(); // Calls the returned method
        }
    }




}
