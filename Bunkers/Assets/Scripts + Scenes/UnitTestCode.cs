using UnityEngine;
using System.Collections;

public class UnitTestCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestCode());
    }

    IEnumerator TestCode()
    {
        // Waits for 2 seconds
        yield return new WaitForSeconds(2);

    }
}
