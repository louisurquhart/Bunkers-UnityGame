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

    // Additive game menu references. They're encapsulated so they are read only except for by the inspector with the private encapsulated variables serialized meaning inspector can still modify them.

    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private GameObject pauseMenuUI;

    public GameObject EndMenuUI // Read only property
    {
        get { return endMenuUI; }
    }

    public GameObject PauseMenuUI // Read only property
    {
        get { return pauseMenuUI; }
    }


    // Class instance references. They're encapsulated so they are read only except for by the inspector with the private encapsulated variables serialized meaning inspector can still modify them.

    [SerializeField] private TimerScript timerScriptInstance;
    [SerializeField] private GeneralBackgroundLogic generalBackgroundLogicInstance;

    public TimerScript TimerScriptInstance // Read only property
    {
        get { return timerScriptInstance; }
    }

    public GeneralBackgroundLogic GeneralBackgroundLogicInstance // Read only property
    {
        get { return generalBackgroundLogicInstance; }
    }

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
}
