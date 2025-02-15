using UnityEngine;

public class FullBunker
{
    // Bunker properties. All encapsulated as they are either read only to external classes, require validation or for rows + columns totalAliveBunkers needs recalculating when set.

    // Rows property. Recalculates totalAliveBunkers when set
    private int _rows;
    public int Rows
    {
        get { return _rows; }
        set { _rows = value; _totalAliveBunkers = Rows * Columns; }
    }

    // Columns property. Recalculates totalAliveBunkers when set
    private int _columns;
    public int Columns
    {
        get { return _columns; }
        set { _columns = value; _totalAliveBunkers = Rows * Columns; }
    }

    // BaseRow property. The row which the bunker starts from
    private int _baseRow;
    public int BaseRow
    {
        get { return _baseRow; }
        set { _baseRow = value; }
    }

    // BaseCol property. The column which the bunker starts from
    private int _baseCol;
    public int BaseCol
    {
        get { return _baseCol; }
        set { _baseCol = value; }
    }

    // TotalBunkers property. Validates that any external modification is decrementing it.
    private int _totalAliveBunkers;
    public int TotalAliveBunkers
    {
        get { return _totalAliveBunkers; }
        set
        {
            if (value == _totalAliveBunkers - 1) { _totalAliveBunkers = value; } // Validates that it's decremeneting total bunkers before setting
            else { Debug.LogWarning($"TotalALiveBunker validation failiure. (Value given: {value}). No value set."); } // If validation fails a warning's output
        } 
    }

    // BunkerColor property. Read only to external classes
    private Color _bunkerColor;
    public Color BunkerColor
    {
        get { return _bunkerColor; }
    }

    // GridManager property. Read only to external classes
    private GridManager _gridManagerRef;
    public GridManager GridManagerRef
    {
        get { return _gridManagerRef; }
    }

    public FullBunker(int givenRows, int givenColumns, Color givenColor, GridManager givenGridManager) // Constructor to instantiate a bunker
    {
        // Sets variables to corrosponding given values
        Rows = givenRows;
        Columns = givenColumns;
        _bunkerColor = givenColor;
        _gridManagerRef = givenGridManager;
    }
}
