// AUTHOR: Alistair/Zac
// Handles card selection and players getting their abilities

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

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
    private int[] selectionOrder = new int[] { -1, -1, -1, -1 };
    private int currentIndex = 0;
    private int numOfCardSelected = 0;
    private GameObject selectedCard = null;
    private int traitorIndex = -1;
    [SerializeField]
    private TMP_Text text;

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
                cardList[traitorIndex].GetComponent<CardHandler>().SwapCard(oldSprite, oldAbility);
            }

            FindAnyObjectByType<AudioManager>().PlaySoundJingle("TraitorFound");
        }

        yield return new WaitForSeconds(0.5f);
        foreach (GameObject card in cardList)
        {
            StartCoroutine(card.GetComponent<CardHandler>().ChangeSprite());
        }

        yield return new WaitForSeconds(3f);
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
            GameObject newCard = cards[Random.Range(0,cards.Length)];
            newCard.GetComponent<CardHandler>().SwapCard(traitorCardSprite, null);
            cardList[currentIndex++] = Instantiate(newCard, transform);
        }
        else
        {
            GameObject newCard = cards[Random.Range(0,cards.Length)];
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

        for (int playerIndex = 0; playerIndex < players.Length; ++playerIndex)
        {
            if (!players[playerIndex].isJoined) break;

            text.text = $"Player {playerIndex + 1} is Selecting a Card";

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
            selectedCard.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f);
            selectedCard = null;
            yield return new WaitForSeconds(0.25f);
        }

        foreach (GameObject card in cardList)
        {
            if (card.GetComponent<Button>() != null)
            {
                Destroy(card.GetComponent<Button>());
            }
            card.GetComponent<Image>().color = new Color(1.0f,1.0f,1.0f);
        }

        text.text = "";
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
