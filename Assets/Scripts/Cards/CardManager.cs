// AUTHOR: Zac
// Loads the card scene and handles giving the players items

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    DungeonCamera lastDunCam = null;
    public GameObject cardCanvasPrefab;
    GameObject cardCanvas;
    public BaseTraitor traitorType;

    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    InputActionAsset defaultActions;

    void Awake()
    {
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
    }

    public void SetTraitorType(BaseTraitor type)
    {
        traitorType = type;
    }

    public void ShowCardSelection(DungeonCamera lastDunCam, int isFinalRoom)
    {
        cardCanvas = Instantiate(cardCanvasPrefab);
        this.lastDunCam = lastDunCam;
        cardCanvas.GetComponentInChildren<CardSelection>().SelectionSetup();
        cardCanvas.GetComponentInChildren<CardSelection>().SetIsFinalRoom(isFinalRoom);
    }

    public void HidePlayer(GameObject player)
    {
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Locked");

        foreach (Transform child in player.transform)
        {
            if (child.GetComponent<WeaponStats>())
            {
                Destroy(child.gameObject);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ShowPlayers()
    {
        PlayerData[] players = PlayerManager.instance.GetPlayers();

        foreach (PlayerData player in players)
        {
            if (!player.isJoined) continue;

            GameObject playerObj = player.playerInput.gameObject;

            playerObj.GetComponent<Animator>().enabled = true;
            playerObj.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 1);
            playerObj.GetComponent<Collider2D>().enabled = true;
            playerObj.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
            playerObj.GetComponent<PlayerScore>().SetTimeStarted();

            foreach (Transform child in playerObj.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void ResumeGameplay(int[] selectionOrder, GameObject[] cardList)
    {
        Destroy(cardCanvas);
        if (lastDunCam != null)
        {
            lastDunCam.RoomChangeTime();
        }
        else
        {
            ShowPlayers();
            FindAnyObjectByType<DungeonBuilder>().GetSpawnedRooms()[0].SetActive(true);
        }
        lastDunCam = null;

        GameObject[] players = new GameObject[4];

        foreach (PlayerData player in PlayerManager.instance.players)
        {
            if (!player.isJoined) continue;
            players[player.playerIndex] = player.playerInput.gameObject;
        }

        // Grant players their items
        for (int i = 0; i < selectionOrder.Length; ++i)
        {
            int abilityIndex = selectionOrder[i];
            if (abilityIndex == -1) continue;

            GameObject abilityObject = cardList[abilityIndex].GetComponent<Card>().abilityObject;

            if (abilityObject == null)
            {
                players[i].AddComponent(traitorType.GetType());
                players[i].GetComponent<PlayerScore>().SetTraitor();
            }
            else if (abilityObject.name.Equals("Pass"))
            {
                continue;
            }
            else if (abilityObject.GetComponent<WeaponStats>())
            {
                players[i].GetComponent<PlayerAttack>().currentWeapon = abilityObject;
                players[i].GetComponent<PlayerHUD>().SetPrimarySprite(cardList[abilityIndex].GetComponent<Card>().cardFrontSprite);
                players[i].GetComponent<PlayerScore>().IncrementWeaponsPicked();
            }
            else if (abilityObject.GetComponent<SecondaryStats>())
            {
                players[i].GetComponent<PlayerSecondary>().currentSecondary = abilityObject;
                players[i].GetComponent<PlayerHUD>().SetSecondarySprite(cardList[abilityIndex].GetComponent<Card>().cardFrontSprite);
                players[i].GetComponent<PlayerScore>().IncrementSecondariesPicked();
            }
            else if (abilityObject.GetComponent<PassiveStats>())
            {
                players[i].GetComponent<PlayerStats>().SetPassive(abilityObject);
                players[i].GetComponent<PlayerHUD>().UpdateStatsDisplay();
                players[i].GetComponent<HealthComponent>().UpdateHUDHealthBar();
                players[i].GetComponent<PlayerScore>().IncrementPassivesPicked();
            }
        }

        int roomNum = FindAnyObjectByType<DungeonManager>().GetRoomCount();

        if (roomNum == 0)
        {
            FindAnyObjectByType<PopupManager>().SpawnLargePopup("Press Left To Attack", Color.white);
        } else if (roomNum == 1)
        {
            FindAnyObjectByType<PopupManager>().SpawnLargePopup("Press Down To Use Secondary", Color.white);
        }

        UIInputModule.actionsAsset = defaultActions;
        UIInputModule.point = InputActionReference.Create(defaultActions.FindAction("UI/Point"));
        UIInputModule.leftClick = InputActionReference.Create(defaultActions.FindAction("UI/Click"));
    }
}
