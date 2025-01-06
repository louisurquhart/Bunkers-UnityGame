using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


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
    [SerializeField] TimerScript timerScriptInstance;
    public TimerScript TimerScriptInstance // Read only property
    {
        get { return timerScriptInstance; }
    }

    // AdditiveGameMenus instance property
    [SerializeField] AdditiveGameMenus additiveGameMenusInstance;
    public AdditiveGameMenus AdditiveGameMenusInstance
    {
        get { return additiveGameMenusInstance; }
    }

    // Game scene event system parent GameObject reference property:
    [SerializeField] private GameObject gameSceneEventSystemParent;
    public GameObject GameSceneEventSystemParent // Read only property
    {
        get { return gameSceneEventSystemParent; }
    }

    // Game scene AudioListener component reference property:
    [SerializeField] private AudioListener gameSceneAudioListener;
    public AudioListener GameSceneAudioListener
    {
        get { return gameSceneAudioListener; }
    }

    //
    [SerializeField] private GridManager playerGridManager;
    public GridManager PlayerGridManager
    {
        get { return playerGridManager; }
    }
}