using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject setupMenu;
    [SerializeField]
    private GameObject optionsMenu;
    
    public void LoadMainMenu()
    {
        mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void LoadSetupMenu()
    {
        setupMenu.SetActive(true);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void LoadOptionsMenu()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        setupMenu.SetActive(false);
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
