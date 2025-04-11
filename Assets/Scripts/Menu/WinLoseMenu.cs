using UnityEngine.SceneManagement;
using UnityEngine;

public class WinLoseMenu : MonoBehaviour
{
    public void LoadMainMenu() 
    {
        //Application.Quit();
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadResultsScene() {
        SceneManager.LoadScene("ResultsScene");
    }
}