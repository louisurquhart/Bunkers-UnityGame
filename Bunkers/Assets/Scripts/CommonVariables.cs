using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommonVariables : MonoBehaviour // a class for public static variables which need to remain consistant while a game's active and there can't be multiple instances of them
{
    // Game/Turn state properties
    public static bool GameActive = false; // Boolean property to signify if a game's active or not. Used by some methods for validation and to improve security
    public static bool PlayerTurn = true;  // Boolean property to signify it's currently the players turn (true by default as player starts by default)
    public static bool Paused = false; // Boolean property to signify if the game is paused or not

    // Score properties
    static public int PlayerScore = 0;
    static public int AIScore = 0;
}
