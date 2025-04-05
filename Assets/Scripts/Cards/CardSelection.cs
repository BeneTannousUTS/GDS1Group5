// AUTHOR: Zac
// Handles card selection process

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;

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
    PlayerData[] playerSelectionOrder = new PlayerData[4];
    private int numOfCardSelected = 0;
    private GameObject selectedCard = null;
    private int currentPlayerIndex = -1;
    [SerializeField]
    private TMP_Text bottomText;
    [SerializeField]
    private GameObject selectingParent;
    [SerializeField]
    private TMP_Text middleText;
    [SerializeField]
    private Image selectingImage;
    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    Sprite[] playerSprites; // this will need to be changed once score order is implemented

    void Awake()
    {
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
    }

    // Flips all cards
    public IEnumerator FlipAll()
    {
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

    public void SelectionSetup()
    {
        List<GameObject> tempCardList = new List<GameObject>();

        GameObject weapon = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Weapon)
            .FirstOrDefault();

        GameObject secondary = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Secondary)
            .FirstOrDefault();

        GameObject passive = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Passive)
            .FirstOrDefault();

        if (weapon != null) tempCardList.Add(weapon);
        if (passive != null) tempCardList.Add(passive);
        if (secondary != null) tempCardList.Add(secondary);

        List<GameObject> remainingCards = cards.Except(tempCardList).ToList();
        tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
        tempCardList = tempCardList.OrderBy(x => Random.value).ToList();

        int i = 0;
        foreach (GameObject card in tempCardList)
        {
            cardList[i++] = Instantiate(card, transform);
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
        // This array will need to be reorder with score once that is implemented...
        playerSelectionOrder = FindAnyObjectByType<PlayerManager>().GetPlayers();
        currentPlayerIndex = -1;

        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardHandler>().showNameAsType();
        }

        for (int playerIndex = 0; playerIndex < playerSelectionOrder.Length; ++playerIndex)
        {
            if (!playerSelectionOrder[playerIndex].isJoined) break;

            foreach (GameObject card in cardList)
            {
                card.GetComponent<CardHandler>().setArrowIcon(null); // this will later be changed to an arrow of the player's colour
                card.GetComponent<CardHandler>().OnDeselect(null);
            }

            bottomText.gameObject.SetActive(false);
            middleText.text = $"Player {playerIndex + 1} is Selecting a Card";
            selectingImage.sprite = playerSprites[playerIndex];
            selectingParent.SetActive(true);

            //VibrateController(playerSelectionOrder[playerIndex].playerInput);

            yield return new WaitForSeconds(2.0f);

            selectingParent.SetActive(false);
            bottomText.gameObject.SetActive(true);

            currentPlayerIndex = playerIndex;

            bottomText.text = $"Player {playerIndex + 1} is Selecting a Card";

            foreach (GameObject card in cardList)
            {
                if (card.GetComponent<Button>() != null)
                {
                    card.GetComponent<Button>().Select();
                    break;
                }
            }

            //Debug.Log(UIInputModule);
            //Debug.Log(playerSelectionOrder[playerIndex].playerInput.actions);

            UIInputModule.actionsAsset = playerSelectionOrder[playerIndex].playerInput.actions;

            for (int pIndex = 0; pIndex < playerSelectionOrder.Length; ++pIndex)
            {
                if (!playerSelectionOrder[pIndex].isJoined) break;

                if (pIndex == playerIndex)
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("CardSelection");
                }
                else
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("Locked");
                }
            }

            yield return new WaitUntil(() => selectedCard != null);

            Destroy(selectedCard.GetComponent<Button>());
            selectedCard.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
            selectedCard.GetComponent<CardHandler>().OnDeselect(null);
            selectedCard = null;
            yield return new WaitForSeconds(0.6f);
        }

        foreach (GameObject card in cardList)
        {
            if (card.GetComponent<Button>() != null)
            {
                Destroy(card.GetComponent<Button>());
            }
            card.GetComponent<CardHandler>().OnDeselect(null);
            card.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        bottomText.text = "";
        currentPlayerIndex = -1;

        DetermineCards();

        // once all players have selected cards flip over
        StartCoroutine(FlipAll());
    }

    void DetermineCards()
    {
        Debug.Log("Determine Card");

        List<int> cardIndices = new List<int> { 0, 1, 2, 3 };

        // at playerSelectionPos = 0, selectionOrder[0] will hold the index of which card in the card list that player selected
        for (int playerSelectionPos = 0; playerSelectionPos < selectionOrder.Length; playerSelectionPos++)
        {
            Debug.Log($"Player Selection Pos: {playerSelectionOrder}");
            int cardIndex = selectionOrder[playerSelectionPos];

            cardIndices.Remove(cardIndex);

            if (cardIndex == -1)
            {
                int newIndex = cardIndices[0];
                cardIndices.Remove(newIndex);
                CardType ranCardType = cardList[newIndex].GetComponent<Card>().cardType;

                GameObject[] cardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == ranCardType)
                    .ToArray();
                Card replacementCard = cardsOfSameType[Random.Range(0, cardsOfSameType.Length)].GetComponent<Card>();
                cardList[newIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
                continue;
            }

            CardType cardType = cardList[cardIndex].GetComponent<Card>().cardType;

            if (cardType == CardType.Weapon)
            {
                GameObject filterWeapon = playerSelectionOrder[playerSelectionPos].playerInput.gameObject.GetComponent<PlayerAttack>().currentWeapon;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterWeapon)
                    .ToArray();
                Card replacementCard = filteredCardsOfSameType[Random.Range(0, filteredCardsOfSameType.Length)].GetComponent<Card>();
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else if (cardType == CardType.Secondary)
            {
                GameObject filterSecondary = playerSelectionOrder[playerSelectionPos].playerInput.gameObject.GetComponent<PlayerSecondary>().currentSecondary;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterSecondary)
                    .ToArray();
                Card replacementCard = filteredCardsOfSameType[Random.Range(0, filteredCardsOfSameType.Length)].GetComponent<Card>();
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else
            {
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType)
                    .ToArray();
                Card replacementCard = filteredCardsOfSameType[Random.Range(0, filteredCardsOfSameType.Length)].GetComponent<Card>();
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
        }

        if (numOfTraitors > 0)
        {
            if (numOfTraitors > numOfCardSelected && numOfTraitors != 4)
            {
                Debug.LogError("More traitors than players!!! This should not happen! Someone messed up >:(");
                return;
            }

            if (numOfTraitors == 4)
            {
                for (int i = 0; i < numOfTraitors; ++i)
                {
                    cardList[i].GetComponent<CardHandler>().setTraitorCard(traitorCardSprite);
                }
            }
            else
            {
                int[] traitorOrder = selectionOrder
                    .Where(numOfCardSelected => numOfCardSelected != -1)
                    .OrderBy(player => Random.value)
                    .ToArray();

                for (int i = 0; i < numOfTraitors; ++i)
                {
                    int traitorIndex = traitorOrder[i];

                    cardList[traitorIndex].GetComponent<CardHandler>().setTraitorCard(traitorCardSprite);
                }
            }

            FindAnyObjectByType<AudioManager>().PlaySoundJingle("TraitorFound");
        }
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

    void VibrateController(PlayerInput playerInput)
    {
        Gamepad gamepad = playerInput.devices.OfType<Gamepad>().FirstOrDefault();
        if (gamepad != null)
        {
            string productName = gamepad.description.product.ToLower();
            Debug.Log("Gamepad product name: " + productName);
            if (productName.Contains("dualshock") || productName.Contains("dualsense"))
            {
                gamepad.SetMotorSpeeds(0.5f, 0.5f);
                StartCoroutine(StopVibrations(gamepad, 0.5f));
            }
        }
    }

    IEnumerator StopVibrations(Gamepad gamepad, float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.ResetHaptics();
    }
}
