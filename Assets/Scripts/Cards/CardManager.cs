// AUTHOR: Zac
// Loads the card scene and handles giving the players items

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    DungeonCamera lastDunCam = null;
    Camera gameSceneCam;
    void Start()
    {
        gameSceneCam = Camera.main;
    }
    public void CardSceneCoroutine(DungeonCamera lastDunCam)
    {
        StartCoroutine(LoadCardScene(lastDunCam));
    }
    IEnumerator LoadCardScene(DungeonCamera lastDunCam)
    {
        gameSceneCam.gameObject.SetActive(false);
        AsyncOperation loadCardScene = SceneManager.LoadSceneAsync("CardSelection", LoadSceneMode.Additive);
        this.lastDunCam = lastDunCam;
        yield return null;
    }

    public void ReloadGameScene()
    {
        // grant players their items in the other scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        SceneManager.UnloadSceneAsync("CardSelection");
        gameSceneCam.gameObject.SetActive(true);
        lastDunCam.RoomChangeTime(gameSceneCam);
        lastDunCam = null;
    }
}
