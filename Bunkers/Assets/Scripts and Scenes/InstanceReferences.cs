using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InstanceReferences : MonoBehaviour
{
    public static InstanceReferences Instance;
    private void Awake()
    {
        Instance = this;

        //GameObject eventSystem = GameObject.Find("EventSystem");
        //Instance = eventSystem.GetComponent<InstanceReferences>();
    }

    // Timer text references
    public TMP_Text PlayerTimerText;
    public TMP_Text AITimerText;

    public GameObject EndMenuUI;
    public GameObject PauseMenuUI;
    
   
    public TimerScript TimerScriptInstance;
    public GeneralBackgroundLogic GeneralBackgroundLogicInstance;

}

