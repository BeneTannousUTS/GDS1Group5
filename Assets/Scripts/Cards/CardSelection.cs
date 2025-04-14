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
using UnityEngine.EventSystems;

public enum CardType
{
    Weapon,
    Secondary,
    Passive
}

public class CardSelection : MonoBehaviour
{
    public GameObject[] cards;
    [SerializeField] Card reviveCard; //specific reference to the revive card because it should not be apart of the regualr card set
    [SerializeField]
    GameObject passObject;
    public Sprite traitorCardSprite;
    private int numOfTraitors = 0;
    [SerializeField]
    private GameObject[] cardList = new GameObject[4];
    private int[] selectedCards = new int[] { -1, -1, -1, -1 }; // player 1's card index will be in the first slot.
    PlayerData[] playerSelectionOrder = new PlayerData[4];
    private GameObject selectedCard = null;
    private int currentPlayerIndex = -1;
    [SerializeField]
    private TMP_Text bottomText;
    [SerializeField]
    private GameObject bottomGroup;
    [SerializeField]
    private GameObject selectingParent;
    [SerializeField]
    private TMP_Text middleText;
    [SerializeField]
    private Image selectingImage;
    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    GameObject traitorCanvasPrefab;
    [SerializeField]
    Sprite[] playerSprites; // this will need to be changed once score order is implemented
    //private float[] oldPlayerScores = new float[4];

    void Awake()
    {
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
        UIInputModule.actionsAsset = null;
        UIInputModule.point = null;
        UIInputModule.leftClick = null;
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

        yield return new WaitForSeconds(3f);

        // need an await until all players have selected here

        if (numOfTraitors > 0)
        {
            StartCoroutine(ShowTraitorCanvas(FindAnyObjectByType<CardManager>().traitorType));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            FindAnyObjectByType<CardManager>().ResumeGameplay(selectedCards, cardList);
        }
    }

    // Sets the value of final room
    public void SetIsFinalRoom(int num)
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

        // int numOfJoinedPlayers = 0;

        // foreach (PlayerData playerData in FindAnyObjectByType<PlayerManager>().GetPlayers())
        // {
        //     if (playerData.isJoined) numOfJoinedPlayers++;
        // }

        // if (numOfJoinedPlayers >= 4)
        // {
        //     remainingCards = cards.Except(tempCardList).ToList();
        //     tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
        // }

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
        playerSelectionOrder = FindAnyObjectByType<PlayerManager>().GetPlayers()
            .Where(playerData => playerData.isJoined)
            .OrderBy(playerData => playerData.playerInput.gameObject.GetComponent<HealthComponent>().GetIsDead())
            .ThenByDescending(playerData => playerData.playerInput.gameObject.GetComponent<PlayerScore>().GetScore()
            /*{
                float currentScore = playerData.playerInput.gameObject.GetComponent<PlayerScore>().GetScore();
                float scoreChange = currentScore - oldPlayerScores[playerData.playerIndex];
                return scoreChange;
            }*/)
            .ToArray();
        currentPlayerIndex = -1;

        // foreach (PlayerData playerData in playerSelectionOrder)
        // {
        //     float currentScore = playerData.playerInput.gameObject.GetComponent<PlayerScore>().GetScore();
        //     float scoreChange = currentScore - oldPlayerScores[playerData.playerIndex];
        //     Debug.Log($"Player {playerData.playerIndex + 1} score change: {scoreChange}");
        //     oldPlayerScores[playerData.playerIndex] = currentScore;
        // }

        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardHandler>().showNameAsType();
        }

        for (int playerSelectionPos = 0; playerSelectionPos < playerSelectionOrder.Length; ++playerSelectionPos)
        {
            if (!playerSelectionOrder[playerSelectionPos].isJoined) break;

            int playerIndex = playerSelectionOrder[playerSelectionPos].playerIndex;

            foreach (GameObject card in cardList)
            {
                card.GetComponent<CardHandler>().setArrowIcon(null); // this will later be changed to an arrow of the player's colour
                card.GetComponent<CardHandler>().OnDeselect(null);
            }

            bottomGroup.SetActive(false);
            bottomText.gameObject.SetActive(false);
            middleText.text = $"Player {playerIndex + 1} is Selecting a Card \nScore: {playerSelectionOrder[playerSelectionPos].playerInput.gameObject.GetComponent<PlayerScore>().GetScore()}";
            selectingImage.sprite = playerSprites[playerIndex];
            selectingParent.SetActive(true);

            for (int pIndex = 0; pIndex < playerSelectionOrder.Length; ++pIndex)
            {
                if (!playerSelectionOrder[pIndex].isJoined) break;

                if (pIndex == playerSelectionPos)
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("Skip");
                }
                else
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("Locked");
                }
            }

            InputActionAsset playerActions = playerSelectionOrder[playerSelectionPos].playerInput.actions;
            UIInputModule.actionsAsset = playerActions;

            yield return WaitForContinue();

            UIInputModule.move = InputActionReference.Create(playerActions.FindAction("CardSelection/CardNav"));
            UIInputModule.submit = InputActionReference.Create(playerActions.FindAction("CardSelection/CardSelect"));

            EventSystem.current.SetSelectedGameObject(null);

            playerSelectionOrder[playerSelectionPos].playerInput.SwitchCurrentActionMap("CardSelection");
            selectingParent.SetActive(false);
            bottomGroup.SetActive(true);
            bottomText.gameObject.SetActive(true);

            currentPlayerIndex = playerIndex;

            bottomText.text = $"Player {playerIndex + 1} is Selecting a Card";

            foreach (GameObject card in cardList)
            {
                if (card.GetComponent<Button>() != null)
                {
                    card.GetComponent<Button>().Select();
                    card.GetComponent<CardHandler>().OnSelect(null);
                    break;
                }
            }

            // this is the code for the old passing card system

            /*if (numOfTraitors > 0 || playerSelectionOrder[playerSelectionPos].playerInput.gameObject.GetComponent<HealthComponent>().GetIsDead())
            {
                bottomGroup.SetActive(false);
                yield return new WaitUntil(() => selectedCard != null);
            }
            else
            {
                yield return WaitingForPass();
            }

            if (!selectedCard.name.Equals("Pass"))
            {
                Destroy(selectedCard.GetComponent<Button>());
                selectedCard.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
                selectedCard.GetComponent<CardHandler>().OnDeselect(null);
            }
            else
            {
                foreach (GameObject card in cardList)
                {
                    if (card.GetComponent<Button>() != null)
                    {
                        card.GetComponent<Button>().OnDeselect(null);
                        card.GetComponent<CardHandler>().OnDeselect(null);
                    }
                }
            }*/

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

        bottomGroup.SetActive(false);
        bottomText.gameObject.SetActive(false);
        currentPlayerIndex = -1;

        DetermineCards();

        // once all players have selected cards flip over
        StartCoroutine(FlipAll());
    }

    void DetermineCards()
    {
        Debug.Log("Determine Card");

        List<int> cardIndices = new List<int> { 0, 1, 2, 3 };
        PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();

        // at playerSelectionPos = 0, selectionOrder[0] will hold the index of which card in the card list that player selected
        for (int playerIndex = 0; playerIndex < selectedCards.Length; playerIndex++)
        {
            int cardIndex = selectedCards[playerIndex];

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
            //Before cards are decided, check if player is alive and if not make the card a revive card
            if (players[playerIndex].playerInput.gameObject.GetComponent<HealthComponent>().GetIsDead())
            {
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(reviveCard);
            }
            else if (cardType == CardType.Weapon)
            {
                GameObject filterWeapon = players[playerIndex].playerInput.gameObject.GetComponent<PlayerAttack>().currentWeapon;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterWeapon)
                    .ToArray();
                Card replacementCard = filteredCardsOfSameType[Random.Range(0, filteredCardsOfSameType.Length)].GetComponent<Card>();
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else if (cardType == CardType.Secondary)
            {
                GameObject filterSecondary = players[playerIndex].playerInput.gameObject.GetComponent<PlayerSecondary>().currentSecondary;
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
            if (numOfTraitors == 4)
            {
                for (int i = 0; i < numOfTraitors; ++i)
                {
                    cardList[i].GetComponent<CardHandler>().setTraitorCard(traitorCardSprite);
                }
            }
            else
            {
                int[] traitorOrder = selectedCards
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
        Debug.Log($"Card Selected: {card.name}");
        selectedCard = card;
        CardHandler cardHandler = card.GetComponent<CardHandler>();
        if (cardHandler) cardHandler.showPlayerIcon(playerSprites[currentPlayerIndex]);
        int abilityIndex = -1;

        for (int i = 0; i < cardList.Length; ++i)
        {
            if (cardList[i] == selectedCard)
            {
                abilityIndex = i;
                break;
            }
        }

        selectedCards[currentPlayerIndex] = abilityIndex;
    }

    private IEnumerator WaitForContinue()
    {
        float timer = 0f;
        InputAction skipAction = UIInputModule.actionsAsset.FindAction("SkipButton");
        while (true)
        {
            if (skipAction != null && skipAction.triggered)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ShowTraitorCanvas(BaseTraitor traitorType)
    {
        GameObject traitorCanvas = Instantiate(traitorCanvasPrefab, null);
        traitorCanvas.GetComponent<TraitorCanvasManager>().SetTraitorType(traitorType);

        yield return new WaitForSeconds(5f);

        yield return WaitForContinue(); // i want to change this to an all player ready system

        Destroy(traitorCanvas);
        FindAnyObjectByType<CardManager>().ResumeGameplay(selectedCards, cardList);
    }
}
