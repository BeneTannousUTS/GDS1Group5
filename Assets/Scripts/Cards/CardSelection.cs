// AUTHOR: Alistair/Zac
// Handles card selection and players getting their abilities

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.UI;

public enum CardType
{
    Weapon,
    Secondary,
    Passive
}

public class CardSelection : MonoBehaviour
{
    public GameObject[] cards;
    public Sprite traitorCardSprite;
    private int numOfTraitors = 0;
    [SerializeField]
    private GameObject[] cardList = new GameObject[4];
    private int[] selectionOrder = new int[] { -1, -1, -1, -1 }; // player 1's card index will be in the first slot.
    private int currentIndex = 0;
    private int numOfCardSelected = 0;
    private GameObject selectedCard = null;
    private int currentPlayerIndex = -1;
    private int traitorIndex = -1;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    Sprite[] playerSprites;

    void Awake()
    {
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
    }

    // Flips all cards
    public IEnumerator FlipAll()
    {
        if (numOfTraitors > 0)
        {
            // ensure traitor card was selected
            bool isTraitorSelected = false;
            foreach (int i in selectionOrder)
            {
                if (i == traitorIndex)
                {
                    isTraitorSelected = true;
                    cardList[traitorIndex].GetComponent<CardHandler>().setTraitorText();
                }
            }

            Debug.Log($"isTraitorSelected: {isTraitorSelected}");

            if (!isTraitorSelected)
            {
                int swapIndex = Random.Range(0, numOfCardSelected); // the player number which is having their card swapped 
                int initialCardNum = selectionOrder[swapIndex];

                Debug.Log($"Player Index: {swapIndex} | Initial Card Number: {initialCardNum}");

                Sprite oldSprite = cardList[initialCardNum].GetComponent<Image>().sprite;
                GameObject oldAbility = cardList[initialCardNum].GetComponent<Card>().abilityObject;
                cardList[initialCardNum].GetComponent<CardHandler>().SwapCard(traitorCardSprite, null);
                cardList[initialCardNum].GetComponent<CardHandler>().setTraitorText();
                cardList[traitorIndex].GetComponent<CardHandler>().SwapCard(oldSprite, oldAbility);
            }

            FindAnyObjectByType<AudioManager>().PlaySoundJingle("TraitorFound");
        }

        yield return new WaitForSeconds(0.5f);
        foreach (GameObject card in cardList)
        {
            StartCoroutine(card.GetComponent<CardHandler>().ChangeSprite());
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardHandler>().showNameAsCard();
            card.GetComponent<CardHandler>().showDesc();
        }        

        yield return new WaitForSeconds(4f);
        FindAnyObjectByType<CardManager>().ResumeGameplay(selectionOrder, cardList);
    }

    // Sets the value of final room
    void SetIsFinalRoom(int num)
    {
        numOfTraitors = num;
    }

    // Instantiates a card and sets the item it will be
    void CreateCard(GameObject cardType, Sprite cardFront)
    {
        cardList[currentIndex] = Instantiate(cardType, transform);
        //cardList[currentIndex].GetComponent<CardHandler>().SetFrontSprite(cardFront);
        currentIndex += 1;
    }

    // Handles the randomness for cards and the calls to CreateCard
    void GetRandomCard(bool isTraitor)
    {
        if (isTraitor == true)
        {
            GameObject newCard = cards[Random.Range(0, cards.Length)];
            cardList[currentIndex++] = Instantiate(newCard, transform);
            cardList[currentIndex - 1].GetComponent<CardHandler>().SwapCard(traitorCardSprite, null);
        }
        else
        {
            GameObject newCard = cards[Random.Range(0, cards.Length)];
            cardList[currentIndex++] = Instantiate(newCard, transform);
        }
    }

    public void SelectionSetup()
    {
        if (numOfTraitors == 1)
        {
            traitorIndex = Random.Range(0, 4);

            GetRandomCard((traitorIndex == 0));
            GetRandomCard((traitorIndex == 1));
            GetRandomCard((traitorIndex == 2));
            GetRandomCard((traitorIndex == 3));
        }
        else if (numOfTraitors == 3)
        {
            traitorIndex = Random.Range(0, 4);

            GetRandomCard((traitorIndex != 0));
            GetRandomCard((traitorIndex != 1));
            GetRandomCard((traitorIndex != 2));
            GetRandomCard((traitorIndex != 3));
        }
        else if (numOfTraitors == 4)
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
        currentPlayerIndex = -1;
        // HIDE NORMAL GAME UI?????

        // This array will need to be reorder with score once that is implemented...

        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardHandler>().showNameAsType();
        }

        for (int playerIndex = 0; playerIndex < players.Length; ++playerIndex)
        {
            if (!players[playerIndex].isJoined) break;

            currentPlayerIndex = playerIndex;

            text.text = $"Player {playerIndex + 1} is Selecting a Card";

            foreach (GameObject card in cardList)
            {
                if (card.GetComponent<Button>() != null)
                {
                    card.GetComponent<Button>().Select();
                    break;
                }
            }

            Debug.Log(UIInputModule);
            Debug.Log(players[playerIndex].playerInput.actions);

            UIInputModule.actionsAsset = players[playerIndex].playerInput.actions;

            for (int pIndex = 0; pIndex < players.Length; ++pIndex)
            {
                if (!players[pIndex].isJoined) break;

                if (pIndex == playerIndex)
                {
                    players[pIndex].playerInput.SwitchCurrentActionMap("CardSelection");
                }
                else
                {
                    players[pIndex].playerInput.SwitchCurrentActionMap("Locked");
                }
            }

            yield return new WaitUntil(() => selectedCard != null);

            Destroy(selectedCard.GetComponent<Button>());
            selectedCard.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            selectedCard = null;
            yield return new WaitForSeconds(0.25f);
        }

        foreach (GameObject card in cardList)
        {
            if (card.GetComponent<Button>() != null)
            {
                Destroy(card.GetComponent<Button>());
            }
            card.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        text.text = "";
        currentPlayerIndex = -1;

        // once all players have selected cards flip over
        StartCoroutine(FlipAll());
    }

    public void SelectCard(GameObject card)
    {
        selectedCard = card;
        card.GetComponent<CardHandler>().showPlayerIcon(playerSprites[currentPlayerIndex]);
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
}
