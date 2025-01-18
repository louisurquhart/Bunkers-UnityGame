using UnityEngine;

public class FullBunker
{
    // Bunker properties. All encapsulated as they are either read only to external classes, require validation or for rows + columns totalAliveBunkers needs recalculating when set.

    // Rows property. Recalculates totalAliveBunkers when set
    private int rows;
    public int Rows
    {
        get { return rows; }
        set { rows = value; totalAliveBunkers = Rows * Columns; }
    }

    // Columns property. Recalculates totalAliveBunkers when set
    private int columns;
    public int Columns
    {
        get { return columns; }
        set { columns = value; totalAliveBunkers = Rows * Columns; }
    }

    private int baseRow;
    public int BaseRow
    {
        get { return baseRow; }
        set { baseRow = value; }
    }

    private int baseCol;
    public int BaseCol
    {
        get { return baseCol; }
        set { baseCol = value; }
    }

    // TotalBunkers property. Validates that any external modification is decrementing it.
    private int totalAliveBunkers;
    public int TotalAliveBunkers
    {
        get { return totalAliveBunkers; }
        set
        {
            if (value == totalAliveBunkers - 1) { totalAliveBunkers = value; }
            else { Debug.LogWarning($"TotalALiveBunker validation failiure. (Value given: {value}). No value set."); } // If validation fails a warning's output
        } // Validates that it's decremeneting total bunkers before setting
    }

    // NumberIdentifer property. Read only to external classes
    private int numberIdentifier;

    // BunkerColor property. Read only to external classes
    private Color bunkerColor;
    public Color BunkerColor
    {
        get { return bunkerColor; }
    }

    // GridManager property. Read only to external classes
    private GridManager gridManagerRef;
    public GridManager GridManagerRef
    {
        get { return gridManagerRef; }
    }

    // Array to store all tiles which are apart of the full bunker
    public Tile[] bunkerTilesArray;


    public FullBunker(int givenRows, int givenColumns, Color givenColor, GridManager givenGridManager) // Constructor to instantiate a bunker
    {
        // Sets variables to corrosponding given values
        Rows = givenRows;
        Columns = givenColumns;
        bunkerColor = givenColor;
        gridManagerRef = givenGridManager;
    }
}
