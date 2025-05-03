// AUTHOR: Julian
// Prompts the results manager to display the results

using UnityEngine;

public class ResultsMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<ResultsManager>().OnSceneLoaded();
        Destroy(GameObject.Find("PlayerHUDCanvas"));
        Destroy(GameObject.Find("ConfirmManager"));
        
    }

    public void ResetResults() {
        Destroy(FindAnyObjectByType<ResultsManager>());
    }
}
