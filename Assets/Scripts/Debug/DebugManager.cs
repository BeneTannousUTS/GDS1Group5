using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    bool debugMode;
    [SerializeField] Canvas debugCanvas;

    void debugStart()
    {
        debugMode = !debugMode;
        if (debugMode)
        {
            debugCanvas.gameObject.SetActive(true);
        }
        else
        {
            debugCanvas.gameObject.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            debugStart();
        }
    }
}
