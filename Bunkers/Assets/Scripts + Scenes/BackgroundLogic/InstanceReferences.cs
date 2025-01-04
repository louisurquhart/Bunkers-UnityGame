using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstanceReferences : MonoBehaviour
{
    // Creates an instance of this class so it can be referenced by static methods
    public static InstanceReferences Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Timer text references. They're encapsulated so they are read only except for by the inspector with the private encapsulated variables serialized meaning inspector can still modify them.
    [SerializeField] private TMP_Text playerTimerText;
    [SerializeField] private TMP_Text aiTimerText;

    public TMP_Text PlayerTimerText // Read only property
    {
        get { return playerTimerText; }
    }

    public TMP_Text AITimerText // Read only property
    {
        get { return aiTimerText; }
    }

    // Pause menu UI reference They're encapsulated so they are read only except for by the inspector with the private encapsulated variables serialized meaning inspector can still modify them.


    [SerializeField] private GameObject pauseMenuUI;

    public GameObject PauseMenuUI // Read only property
    {
        get { return pauseMenuUI; }
    }


    // Class instance references. They're encapsulated so they are read only except for by the inspector with the private encapsulated variables serialized meaning inspector can still modify them.

    [SerializeField] private TimerScript timerScriptInstance;

    public TimerScript TimerScriptInstance // Read only property
    {
        get { return timerScriptInstance; }
    }

    //[SerializeField] private GeneralBackgroundLogic generalBackgroundLogicInstance;

    //public GeneralBackgroundLogic GeneralBackgroundLogicInstance // Read only property
    //{
    //    get { return generalBackgroundLogicInstance; }
    //}

    // GameScene event system reference

    [SerializeField] private GameObject gameSceneEventSystemParent;
    public GameObject GameSceneEventSystemParent // Read only property
    {
        get { return gameSceneEventSystemParent; }
    }

    [SerializeField] private AudioListener gameSceneAudioListener;

    public AudioListener GameSceneAudioListener
    {
        get { return gameSceneAudioListener; }
    }

    [SerializeField] private GridManager playerGridManager;

    public GridManager PlayerGridManager
    {
        get { return playerGridManager; }
    }


    // ----------- END MENU GAMEOBJECT/TEXT REFERENCES ------------
    [SerializeField] private GameObject endMenuUI;

    public GameObject EndMenuUI // Read only property
    {
        get { return endMenuUI; }
    }

    public TMP_Text EndMenuPlayerScoreTMP;
    public TMP_Text EndMenuAIScoreTMP;


    [SerializeField] TMP_Text endMenuGameOutcomeTMPObject;

    public TMP_Text EndMenuGameOutcomeTMPObject
    {
        get { return endMenuGameOutcomeTMPObject; }
    }

    
}