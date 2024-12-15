using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker
{
    // defines the attributes of a bunker
    public string BunkerName;
    public int Rows;
    public int Columns;
    public Color BunkerColor;

    public Bunker(string givenType, int givenRows, int givenColumns, Color givenColor) // Constructor to instantiate a bunker
    {
        BunkerName = givenType;
        Rows = givenRows;
        Columns = givenColumns;
        BunkerColor = givenColor;
        // will have bunker image in final iteration
    }
}
