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

    public void ReloadGameScene(int[] selectionOrder, GameObject[] abilityList)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        SceneManager.UnloadSceneAsync("CardSelection");
        gameSceneCam.gameObject.SetActive(true);
        lastDunCam.RoomChangeTime(gameSceneCam);
        lastDunCam = null;

        // Grant players their items in the other scene
        for (int i = 0; i < selectionOrder.Length; ++i)
        {
            int abilityIndex = selectionOrder[i];
            // if i = 0 has selection order 2 that means that Player 0 selected the 
            // 3rd card so should get the 3rd ability (which is at index 2)
            GameObject abilityObject = abilityList[abilityIndex];

            if (abilityObject.GetComponent<WeaponStats>())
            {
                PlayerAttack[] playerAttacks = FindObjectsOfType<PlayerAttack>();
                playerAttacks[i].currentWeapon = abilityObject;
            }

            // give player object?
        }
    }
}
