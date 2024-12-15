using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleCommands : MonoBehaviour
{
    public static ConsoleCommands Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void RandomlyGenerateBunkers()
    {
        Debug.Log("RandomlyGenerateBunker command called");
    }
}




