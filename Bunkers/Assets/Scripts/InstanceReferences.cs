using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstanceReferences : MonoBehaviour
{
    public static InstanceReferences instance;
    private void Awake()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        instance = eventSystem.GetComponent<InstanceReferences>();
    }

    // Timer text references
    public GameObject PlayerTimerText;
    public GameObject AITimerText;

    // Timer script instance
    public TimerScript TimerScriptInstance;

}

