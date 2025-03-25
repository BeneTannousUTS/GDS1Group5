// AUTHOR: 
// Handles destruction of warning UI

using UnityEngine;
using System.Collections;

public class WarningUI : MonoBehaviour
{
    // Destroys the warning after 0.4 seconds
    IEnumerator DestroyWarning()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    // Starts DestroyWarning coroutine
    void Start()
    {
        StartCoroutine(DestroyWarning());
    }
}