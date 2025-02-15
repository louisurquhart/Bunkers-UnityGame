using UnityEngine;

public class Tile : MonoBehaviour
{
    // ------- Variables --------
    public int Row; // Row position of tile
    public int Col; // Column position of the tile

    // Tile state variables
    public bool IsBunker = false;  // Value to show if tile's a bunker

    // ------- Encapsulated properties for the tiles attributes --------

    // GridManager property
    protected GridManager _gridManager;
    virtual public GridManager GridManager
    {
        get { return _gridManager; } // Only has getter to make it read only to other classes as only intialize method in the class should modify it.
    }

    // TileSpriteRenderer property 
    [SerializeField] private SpriteRenderer tileSpriteRenderer;
    public SpriteRenderer TileSpriteRenderer
    {
        get { return tileSpriteRenderer; } // Only has getter to make it read only to other classes as only intialize method in the class should modify it.
    }
    // TileColour property
    private Color _tileColour;
    public Color TileColour
    {
        get { return _tileColour; }
        set
        {
            if (value == TileSpriteRenderer.color) { _tileColour = value; } // If the value TileColour is being to set to is actually the tile's colour it sets it
            else { Debug.LogWarning("Given tileColour not synchronized with actual tile colour. (tileColour = {tileColour} actualTileColour = {TileSpriteRenderer.color. No changes made "); } // Otherwise it outputs a warning saying that the input wasn't accepted
        }

    }
    // FullBunkerReference property
    private FullBunker _fullBunkerReference;
    public FullBunker FullBunkerReference
    {
        get
        {
            if (IsBunker) { return _fullBunkerReference; } // If the tile is a bunker it returns the reference to the fullBunker
            else { Debug.LogError($"FullBunkerReference not returned. isBunker == {IsBunker}"); return null; }// Otherwise it returns null as if the tile isn't a bunker it can't have a fullBunker reference + outputs error to signify this} 
        }
        set { _fullBunkerReference = value; }
    }

    // IsPreviouslyHit property
    private bool _isPreviouslyHit;
    public bool IsPreviouslyHit
    {
        get { return _isPreviouslyHit; }
        set
        {
            _isPreviouslyHit = value;
        }
    }

    protected void Awake() // Called immediately after class is created
    {
        // Syncronises the TileColour variable with the tile's actual colour
        TileColour = TileSpriteRenderer.color;
    }

    // Initialise method called by gridManager to create + set the properties of the tile
    public void Initalise(int rowRef, int colRef, GridManager gridManagerRef)
    {
        // Sets corrosponding variables to the given ones
        Row = rowRef;
        Col = colRef;
        _gridManager = gridManagerRef;
        tileSpriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log($"Instantiated: gridManager = {gridManager}");
        // Outputs log for testing
        //Debug.Log($"{CommonVariables.DebugFormat[GridManager.EntityNum]}Initialise: Tile {this} at row: {rowRef}, {colRef} initialized. Rows == {Row}, Columns == {Col}, TileSpriteRenderer == {TileSpriteRenderer}");
    }

    // Method to set the tile as bunker (should be overrided by tile subclasses as it depends on which entities tile it is)
    public virtual void SetBunker(FullBunker givenBunkerType)
    {
        Debug.LogError($"{CommonVariables.DebugFormat[GridManager.EntityNum]}SetBunker: Failiure to override SetBunker method"); // If method's not overrided it outputs error
    }


    // Method to change the tiles colour (should change tileColour value + the actual tiles colour)
    public void UpdateTileColour(Color color, bool temporary)
    {
        if (!temporary)
        {
            _tileColour = color;
            //Debug.Log($"NewTileColour: {tileColour} at row {Row}, column {Col}");
        }
        TileSpriteRenderer.color = color;
    }
}






