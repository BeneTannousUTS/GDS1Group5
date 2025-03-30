// AUTHOR: Alistair
// Handles card selection and players getting their abilities

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum CardType
{
    Weapon,
    Secondary,
    Passive
}

public class CardSelection : MonoBehaviour
{
    public Sprite[] passiveCardSprites;
    public Sprite[] secondaryCardSprites;
    public Sprite[] weaponCardSprites;
    public Sprite traitorCardSprite;

    public GameObject[] secondaryComponents;
    public GameObject[] weaponComponents;
    public GameObject[] passiveComponents;

    public GameObject passiveCard;
    public GameObject secondaryCard;
    public GameObject weaponCard;

    private int traitorNum = 0;
    [SerializeField]
    private GameObject[] cardList = new GameObject[4];
    [SerializeField]
    private GameObject[] abilityList = new GameObject[4];
    private int[] selectionOrder = new int[] { -1, -1, -1, -1 };
    private int currentIndex = 0;
    private int numOfCardSelected = 0;
    private GameObject selectedCard = null;
    private int traitorIndex = -1;

    // Flips all cards
    public IEnumerator FlipAll()
    {
        if (traitorNum > 0)
        {
            // ensure traitor card was selected
            bool isTraitorSelected = false;
            foreach (int i in selectionOrder)
            {
                if (i == traitorIndex)
                {
                    isTraitorSelected = true;
                }
            }

            Debug.Log($"isTraitorSelected: {isTraitorSelected}");

            if (!isTraitorSelected)
            {
                int swapIndex = Random.Range(0, numOfCardSelected); // the player number which is having their card swapped 
                int initialCardNum = selectionOrder[swapIndex];

                Debug.Log($"Player Index: {swapIndex} | Initial Card Number: {initialCardNum}");

                Sprite oldSprite = cardList[initialCardNum].GetComponent<Image>().sprite;
                cardList[initialCardNum].GetComponent<CardHandler>().frontSprite = traitorCardSprite;
                cardList[traitorIndex].GetComponent<CardHandler>().frontSprite = oldSprite;

                GameObject oldAbility = abilityList[initialCardNum];
                abilityList[initialCardNum] = null;
                abilityList[traitorIndex] = oldAbility;
            }

            FindAnyObjectByType<AudioManager>().PlaySoundJingle("TraitorFound");
        }

        yield return new WaitForSeconds(0.5f);
        foreach (GameObject card in cardList)
        {
            StartCoroutine(card.GetComponent<CardHandler>().ChangeSprite());
        }

        yield return new WaitForSeconds(3f);
        FindAnyObjectByType<CardManager>().ReloadGameScene(selectionOrder, abilityList);
    }

    // Sets the value of final room
    void SetIsFinalRoom(int num)
    {
        traitorNum = num;
    }

    // Gets the value of ability list
    public GameObject[] GetAbilityList()
    {
        return abilityList;
    }

    // Instantiates a card and sets the item it will be
    void CreateCard(GameObject cardType, Sprite cardFront)
    {
        cardList[currentIndex] = Instantiate(cardType, transform);
        cardList[currentIndex].GetComponent<CardHandler>().SetFrontSprite(cardFront);
        currentIndex += 1;
    }

    // Handles the randomness for cards and the calls to CreateCard
    void GetRandomCard(bool isTraitor)
    {
        int cardType = Random.Range(0, 3);

        if (isTraitor == true)
        {
            abilityList[currentIndex] = null;

            if (cardType == 0)
            {
                CreateCard(weaponCard, traitorCardSprite);
            }
            else if (cardType == 1)
            {
                CreateCard(secondaryCard, traitorCardSprite);
            }
            else
            {
                CreateCard(passiveCard, traitorCardSprite);
            }

        }
        else
        {
            if (cardType == 0)
            {
                int abilityIndex = Random.Range(0, weaponCardSprites.Length);
                abilityList[currentIndex] = weaponComponents[abilityIndex];
                CreateCard(weaponCard, weaponCardSprites[abilityIndex]);
            }
            else if (cardType == 1)
            {
                int abilityIndex = Random.Range(0, secondaryCardSprites.Length);
                abilityList[currentIndex] = secondaryComponents[abilityIndex];
                CreateCard(secondaryCard, secondaryCardSprites[abilityIndex]);
            }
            else
            {
                int abilityIndex = Random.Range(0, passiveCardSprites.Length);
                abilityList[currentIndex] = passiveComponents[abilityIndex];
                CreateCard(passiveCard, passiveCardSprites[abilityIndex]);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int totalRooms = FindAnyObjectByType<DungeonBuilder>().numberRooms - 1;
        int currentRoom = FindAnyObjectByType<DungeonBuilder>().roomsCleared;

        Debug.Log($"Current Room: {currentRoom} | Total Rooms: {totalRooms}");

        if (totalRooms == currentRoom)
        {
            traitorNum = 1;
        }

        if (traitorNum == 1)
        {
            traitorIndex = Random.Range(0, 4);

            GetRandomCard((traitorIndex == 0));
            GetRandomCard((traitorIndex == 1));
            GetRandomCard((traitorIndex == 2));
            GetRandomCard((traitorIndex == 3));
        }
        else if (traitorNum == 3)
        {
            traitorIndex = Random.Range(0, 4);

            GetRandomCard((traitorIndex != 0));
            GetRandomCard((traitorIndex != 1));
            GetRandomCard((traitorIndex != 2));
            GetRandomCard((traitorIndex != 3));
        }
        else if (traitorNum == 4)
        {
            GetRandomCard(true);
            GetRandomCard(true);
            GetRandomCard(true);
            GetRandomCard(true);
        }
        else
        {
            GetRandomCard(false);
            GetRandomCard(false);
            GetRandomCard(false);
            GetRandomCard(false);
        }

        foreach (GameObject card in cardList)
        {
            Button button = card.GetComponent<Button>();

            button.onClick.AddListener(() => SelectCard(card));
        }

        StartCoroutine(SelectionProcess());
    }

    IEnumerator SelectionProcess()
    {
        // get a list of joined players
        PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();

        for (int playerIndex = 0; playerIndex < players.Length; ++playerIndex)
        {
            if (!players[playerIndex].isJoined) break;

            foreach (GameObject card in cardList)
            {
                if (card.GetComponent<Button>() != null)
                {
                    card.GetComponent<Button>().Select();
                    break;
                }
            }

            //InputFilter.ActiveDevice = players[playerIndex].playerInput.devices[0];

            yield return new WaitUntil(() => selectedCard != null);

            Destroy(selectedCard.GetComponent<Button>());
            selectedCard = null;
            yield return new WaitForSeconds(0.25f);
        }

        foreach (GameObject card in cardList)
        {
            if (card.GetComponent<Button>() != null)
            {
                Destroy(card.GetComponent<Button>());
            }
        }

        // once all players have selected cards flip over
        StartCoroutine(FlipAll());
    }

    public void SelectCard(GameObject card)
    {
        selectedCard = card;
        int abilityIndex = -1;

        for (int i = 0; i < cardList.Length; ++i)
        {
            if (cardList[i] == selectedCard)
            {
                abilityIndex = i;
                break;
            }
        }

        selectionOrder[numOfCardSelected++] = abilityIndex;
    }

    void EnableOnlyPlayer(int index)
    {
        PlayerData[] players = PlayerManager.instance.GetPlayers();

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isJoined || players[i].playerInput == null)
                continue;

            if (i == index)
            {
                if (!players[i].playerInput.enabled)
                    players[i].playerInput.enabled = true;
            }
            else
            {
                players[i].playerInput.DeactivateInput();
            }
        }
    }

    void DisableAllPlayers()
    {
        PlayerData[] players = PlayerManager.instance.GetPlayers();

        foreach (var player in players)
        {
            if (player.isJoined && player.playerInput != null)
            {
                player.playerInput.DeactivateInput();
            }
        }
    }
}
