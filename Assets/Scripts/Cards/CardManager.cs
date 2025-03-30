// AUTHOR: Zac
// Loads the card scene and handles giving the players items
// Determines the role of the traitor

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    DungeonCamera lastDunCam = null;
    Camera gameSceneCam;
    int traitorIndex = -1;
    void Start()
    {
        gameSceneCam = Camera.main;
        DetermineTraitor();
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

        GameObject[] players = new GameObject[4];

        foreach (PlayerData player in PlayerManager.instance.players)
        {
            if (player.playerIndex == -1) break;
            players[player.playerIndex] = player.playerInput.gameObject;
        }

        // Destroy all children of each player (fix for left over weapons?)
        foreach (GameObject player in players)
        {
            if (player == null) continue;

            Transform parentTransform = player.transform;
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = parentTransform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        // Grant players their items
        for (int i = 0; i < selectionOrder.Length; ++i)
        {
            int abilityIndex = selectionOrder[i];
            if (abilityIndex == -1) continue;

            GameObject abilityObject = abilityList[abilityIndex];

            if (abilityObject == null)
            {
                if (traitorIndex == 0)
                {
                    players[i].AddComponent<CloneTraitor>();
                } else if (traitorIndex == 1)
                {
                    foreach (GameObject player in players)
                    {
                        player.AddComponent<PVPTraitor>();
                    }
                }
            }
            else if (abilityObject.GetComponent<WeaponStats>())
            {
                players[i].GetComponent<PlayerAttack>().currentWeapon = abilityObject;
            }
            else if (abilityObject.GetComponent<SecondaryStats>())
            {
                players[i].GetComponent<PlayerSecondary>().currentSecondary = abilityObject;
            }
            else if (abilityObject.GetComponent<PassiveStats>())
            {
                players[i].GetComponent<PlayerStats>().SetPassive(abilityObject);
            }
        }
    }

    void DetermineTraitor()
    {
        traitorIndex = 1;
        return;

        GameObject[] players = new GameObject[4];
        int playerJoinedCount = 0;

        foreach (PlayerData player in PlayerManager.instance.players)
        {
            if (player.isJoined) playerJoinedCount++;
        }

        if (playerJoinedCount < 4)
        {
            traitorIndex = 0;
        } else
        {
            traitorIndex = UnityEngine.Random.Range(0,2);
        }
    }
}
