// AUTHOR: Zac
// Loads the card scene and handles giving the players items

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    DungeonCamera lastDunCam = null;
    public GameObject cardCanvasPrefab;
    GameObject cardCanvas;
    int traitorIndex = 1; // only temp until game manager is active

    public void ShowCardSelection(DungeonCamera lastDunCam)
    {
        cardCanvas = Instantiate(cardCanvasPrefab);
        this.lastDunCam = lastDunCam;
        cardCanvas.GetComponentInChildren<CardSelection>().SelectionSetup();
    }

    public void HidePlayer(GameObject player)
    {
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<SpriteRenderer>().color = new Vector4(1,1,1,0);
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Locked");
    }

    public void ShowPlayers()
    {
        PlayerData[] players = PlayerManager.instance.GetPlayers();

        foreach (PlayerData player in players)
        {
            if (!player.isJoined) break;

            GameObject playerObj = player.playerInput.gameObject;

            playerObj.GetComponent<Animator>().enabled = true;
            playerObj.GetComponent<SpriteRenderer>().color = new Vector4(1,1,1,1);
            playerObj.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        }
    }

    public void ResumeGameplay(int[] selectionOrder, GameObject[] cardList)
    {
        Destroy(cardCanvas);
        lastDunCam.RoomChangeTime();
        lastDunCam = null;

        GameObject[] players = new GameObject[4];

        foreach (PlayerData player in PlayerManager.instance.players)
        {
            if (player.playerIndex == -1) break;
            players[player.playerIndex] = player.playerInput.gameObject;
        }

        // Destroy all children of each player (fix for left over weapons?)
        // Commenting out this code to avoid destroying healthbar - no observed problems so far
        /*foreach (GameObject player in players)
        {
            if (player == null) continue;

            Transform parentTransform = player.transform;
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = parentTransform.GetChild(i);
                Destroy(child.gameObject);
            }
        }*/

        // Grant players their items
        for (int i = 0; i < selectionOrder.Length; ++i)
        {
            int abilityIndex = selectionOrder[i];
            if (abilityIndex == -1) continue;

            GameObject abilityObject = cardList[abilityIndex].GetComponent<Card>().abilityObject;

            if (abilityObject == null)
            {
                if (traitorIndex == 0)
                {
                    players[i].AddComponent<CloneTraitor>();
                }
                else if (traitorIndex == 1)
                {
                    foreach (GameObject player in players)
                    {
                        if (player == null) continue;
                        player.AddComponent<PVPTraitor>();
                    }
                }
            }
            else if (abilityObject.GetComponent<WeaponStats>())
            {
                players[i].GetComponent<PlayerAttack>().currentWeapon = abilityObject;
                players[i].GetComponent<PlayerHUD>().SetPrimarySprite(cardList[abilityIndex].GetComponent<Card>().cardFrontSprite);
            }
            else if (abilityObject.GetComponent<SecondaryStats>())
            {
                players[i].GetComponent<PlayerSecondary>().currentSecondary = abilityObject;
                players[i].GetComponent<PlayerHUD>().SetSecondarySprite(cardList[abilityIndex].GetComponent<Card>().cardFrontSprite);
            }
            else if (abilityObject.GetComponent<PassiveStats>())
            {
                players[i].GetComponent<PlayerStats>().SetPassive(abilityObject);
                players[i].GetComponent<PlayerHUD>().UpdateStatsDisplay();
                players[i].GetComponent<HealthComponent>().UpdateHUDHealthBar();
            }
        }
    }
}
