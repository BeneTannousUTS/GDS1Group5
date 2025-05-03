using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject optionsMenu;

    public void Start()
    {
        FindAnyObjectByType<AudioManager>().PlayMenuTheme();
    }

    public void LoadMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        mainMenu.GetComponentInChildren<Button>().Select();
    }
    
    public void LoadOptionsMenu()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        
        optionsMenu.GetComponentInChildren<Button>().Select();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameLobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
