using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InstanceReferences : MonoBehaviour
{
    // Creates an instance of this class so it can be referenced by static methods
    public static InstanceReferences Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Timer text references
    public TMP_Text PlayerTimerText;
    public TMP_Text AITimerText;

    // Additive game menu references
    public GameObject EndMenuUI;
    public GameObject PauseMenuUI;
    
    // Class instance references
    public TimerScript TimerScriptInstance;
    public GeneralBackgroundLogic GeneralBackgroundLogicInstance;

}

