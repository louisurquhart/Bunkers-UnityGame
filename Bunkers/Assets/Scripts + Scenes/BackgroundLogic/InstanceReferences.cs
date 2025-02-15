using UnityEngine;

public class InstanceReferences : MonoBehaviour // Class which holds references of instances to be used by methods which require to be static - but need to access instance based gameobjects / classes
{
    // Creates an instance of this class so it can be referenced by static methods:
    public static InstanceReferences Instance;

    private void Awake()
    {
        Instance = this; // Sets instance to the class
    }
    
    // ------- ENCAPSULATED READ ONLY PROPERTIES: --------

    // TimerScript instance property
    [SerializeField] TimerScript _timerScriptInstance;
    public TimerScript TimerScriptInstance // Read only property
    {
        get { return _timerScriptInstance; }
    }

    // AdditiveGameMenus instance property
    [SerializeField] AdditiveGameMenus _additiveGameMenusInstance;
    public AdditiveGameMenus AdditiveGameMenusInstance
    {
        get { return _additiveGameMenusInstance; }
    }

    // Game scene event system parent GameObject reference property:
    [SerializeField] private GameObject _gameSceneEventSystemParent;
    public GameObject GameSceneEventSystemParent // Read only property
    {
        get { return _gameSceneEventSystemParent; }
    }

    // Game scene AudioListener component reference property:
    [SerializeField] private AudioListener _gameSceneAudioListener;
    public AudioListener GameSceneAudioListener
    {
        get { return _gameSceneAudioListener; }
    }

    //
    [SerializeField] private GridManager _playerGridManager;
    public GridManager PlayerGridManager
    {
        get { return _playerGridManager; }
    }
}