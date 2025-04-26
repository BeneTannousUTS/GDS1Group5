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

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum SelectionState
{
    InGame,
    ConfirmingTurn,
    Selecting,
    Waiting,
    ConfirmingSwap,
    TraitorConfirming
}

public class CardSelection : MonoBehaviour
{
    public GameObject[] cards;
    // [SerializeField]
    // Card reviveCard; // specific reference to the revive card because it should not be apart of the regualr card set
    // [SerializeField]
    // GameObject passObject;
    [SerializeField]
    GameObject confirmCanvas;
    [SerializeField]
    GameObject coverCanvas;
    [SerializeField]
    GameObject confirmCardPrefab;
    public Sprite traitorCardSprite;
    private int numOfTraitors = 0;
    [SerializeField]
    public GameObject[] cardList = new GameObject[4];
    private int[] selectedCards = new int[] { -1, -1, -1, -1 }; // player 1's card index will be in the first slot.
    PlayerData[] playerSelectionOrder = new PlayerData[4];
    private GameObject selectedCard = null;
    public int currentPlayerIndex = -1;
    [SerializeField]
    private TMP_Text bottomText;
    [SerializeField]
    private GameObject bottomGroup;
    [SerializeField]
    private GameObject continuePromptPrefab;
    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    GameObject traitorCanvasPrefab;
    [SerializeField]
    GameObject damagePassiveCard;
    [SerializeField]
    GameObject cooldownPassiveCard;
    public SelectionState selectionState = SelectionState.InGame;

    void Awake()
    {
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
        UIInputModule.actionsAsset = null;
        UIInputModule.point = null;
        UIInputModule.leftClick = null;
    }

    #region Selection Setup

    public void SelectionSetup()
    {
        List<GameObject> tempCardList = new List<GameObject>();
        int roomNum = FindAnyObjectByType<DungeonManager>().GetRoomCount();
        Debug.Log($"Current Room: {roomNum}");

        GameObject weapon = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Weapon)
            .FirstOrDefault();

        GameObject secondary = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Secondary)
            .FirstOrDefault();

        GameObject passive = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Passive)
            .FirstOrDefault();

        if (roomNum == 1)
        {
            for (int j = 0; j < 4; ++j)
            {
                List<GameObject> remainingCards = cards
                .Where(card => card.GetComponent<Card>().cardType == CardType.Weapon)
                .ToList();

                tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
            }
        }
        else if (roomNum == 2)
        {
            for (int j = 0; j < 4; ++j)
            {
                List<GameObject> remainingCards = cards
                .Where(card => card.GetComponent<Card>().cardType == CardType.Secondary)
                .ToList();

                tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
            }
        }
        else
        {
            if (weapon != null) tempCardList.Add(weapon);
            if (passive != null) tempCardList.Add(passive);
            if (secondary != null) tempCardList.Add(secondary);

            List<GameObject> remainingCards = cards.Except(tempCardList).ToList();
            tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
        }

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

    #endregion

    #region Selection Process

    IEnumerator SelectionProcess()
    {
        // get a list of joined players and sort by score
        playerSelectionOrder = FindAnyObjectByType<PlayerManager>().GetPlayers()
            .Where(playerData => playerData.isJoined)
            .OrderBy(playerData => playerData.playerInput.gameObject.GetComponent<HealthComponent>().GetIsDead())
            .ThenByDescending(playerData => playerData.playerInput.gameObject.GetComponent<PlayerScore>().GetScore())
            .ToArray();
        currentPlayerIndex = -1;

        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardHandler>().showNameAsType();
        }

        for (int playerSelectionPos = 0; playerSelectionPos < playerSelectionOrder.Length; ++playerSelectionPos)
        {
            if (!playerSelectionOrder[playerSelectionPos].isJoined) break;

            selectionState = SelectionState.ConfirmingTurn;

            int playerIndex = playerSelectionOrder[playerSelectionPos].playerIndex;
            currentPlayerIndex = playerIndex;

            foreach (GameObject card in cardList)
            {
                card.GetComponent<CardHandler>().setArrowIcon(playerSelectionOrder[playerSelectionPos].playerColour);
                card.GetComponent<CardHandler>().OnDeselect(null);
            }

            bottomGroup.SetActive(false);
            bottomText.gameObject.SetActive(false);

            GameObject continuePromptObject = Instantiate(continuePromptPrefab, confirmCanvas.transform);
            ContinueConfirmHandler continueHandler = continuePromptObject.GetComponent<ContinueConfirmHandler>();

            continueHandler.init(playerSelectionOrder[playerSelectionPos].playerInput);
            continueHandler.Setup(
                $"Player {playerIndex + 1} is Selecting a Card\nScore: {playerSelectionOrder[playerSelectionPos].playerInput.gameObject.GetComponent<PlayerScore>().GetScore()}",
                PlayerManager.instance.playerSprites[playerIndex]
            );

            for (int pIndex = 0; pIndex < playerSelectionOrder.Length; ++pIndex)
            {
                if (!playerSelectionOrder[pIndex].isJoined) break;

                if (pIndex == playerSelectionPos)
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("Confirm/Skip");
                }
                else
                {
                    playerSelectionOrder[pIndex].playerInput.SwitchCurrentActionMap("Locked");
                }
            }

            InputActionAsset playerActions = playerSelectionOrder[playerSelectionPos].playerInput.actions;
            UIInputModule.actionsAsset = playerActions;

            List<BaseConfirmHandler> handlerList = new List<BaseConfirmHandler> { continueHandler };
            yield return ConfirmManager.Instance.WaitForAllConfirmations(handlerList);

            Destroy(continuePromptObject);

            selectionState = SelectionState.Selecting;

            UIInputModule.move = InputActionReference.Create(playerActions.FindAction("CardSelection/CardNav"));
            UIInputModule.submit = InputActionReference.Create(playerActions.FindAction("CardSelection/CardSelect"));

            EventSystem.current.SetSelectedGameObject(null);

            playerSelectionOrder[playerSelectionPos].playerInput.SwitchCurrentActionMap("CardSelection");
            bottomGroup.SetActive(true);
            bottomText.gameObject.SetActive(true);
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

            yield return new WaitUntil(() => selectedCard != null);

            selectionState = SelectionState.Waiting;

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

    #endregion

    #region Determine Cards

    void DetermineCards()
    {
        List<int> cardIndices = new List<int> { 0, 1, 2, 3 };
        PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();
        float dungeonCompletionPercent = (float)FindAnyObjectByType<DungeonManager>().GetRoomCount() / (float)FindAnyObjectByType<DungeonManager>().GetDungeonLength();

        Debug.Log($"Determine Card | Completion Percentage: {dungeonCompletionPercent * 100}%");

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
                Card replacementCard = GetWeightedRandomCard(cardsOfSameType, dungeonCompletionPercent);
                cardList[newIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
                continue;
            }

            CardType cardType = cardList[cardIndex].GetComponent<Card>().cardType;
            if (cardType == CardType.Weapon)
            {
                GameObject filterWeapon = players[playerIndex].playerInput.gameObject.GetComponent<PlayerAttack>().currentWeapon;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterWeapon)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(filteredCardsOfSameType, dungeonCompletionPercent);
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else if (cardType == CardType.Secondary)
            {
                GameObject filterSecondary = players[playerIndex].playerInput.gameObject.GetComponent<PlayerSecondary>().currentSecondary;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterSecondary)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(filteredCardsOfSameType, dungeonCompletionPercent);
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else
            {
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(filteredCardsOfSameType, dungeonCompletionPercent);
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

    #endregion

    #region FlipAll

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

        yield return new WaitForSeconds(3f); // maybe turn this into a ready check

        UIInputModule.actionsAsset = null;
        int roomNum = FindAnyObjectByType<DungeonManager>().GetRoomCount();

        if (roomNum >= 3)
        {

            PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();
            List<BaseConfirmHandler> handlers = new List<BaseConfirmHandler>(); // at the player's index will hold the confirm card

            foreach (PlayerData playerData in players)
            {
                if (!playerData.isJoined) continue;

                if (cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardType == CardType.Weapon
                || cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardType == CardType.Secondary)
                {
                    coverCanvas.SetActive(true);
                    selectionState = SelectionState.ConfirmingSwap;
                    GameObject confirmCard = Instantiate(confirmCardPrefab, confirmCanvas.transform);
                    playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");

                    ConfirmCardHandler confirmCardHandler = confirmCard.GetComponent<ConfirmCardHandler>();
                    confirmCardHandler.init(playerData.playerInput);

                    confirmCardHandler.SetupCard(
                        $"Player {playerData.playerIndex + 1}",
                        PlayerManager.instance.playerSprites[playerData.playerIndex],
                        cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardType == CardType.Weapon ?
                            playerData.playerInput.GetComponent<PlayerHUD>().GetUIComponentHelper().primaryAbility.sprite :
                            playerData.playerInput.GetComponent<PlayerHUD>().GetUIComponentHelper().secondaryAbility.sprite,
                        cardList[selectedCards[playerData.playerIndex]].GetComponent<Image>().sprite,
                        $"Swap to the {cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardName}.",
                        cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardType == CardType.Weapon ?
                            $"Reject and increase damage." :
                            $"Reject and reduced cooldowns."
                    );

                    handlers.Add(confirmCardHandler);
                }
            }
            yield return ConfirmManager.Instance.WaitForAllConfirmations(handlers);

            foreach (BaseConfirmHandler cardHandler in handlers)
            {
                if (cardHandler.confirmedChoice == false)
                {
                    cardList[selectedCards[cardHandler.playerIndex]] =
                        cardList[selectedCards[cardHandler.playerIndex]].GetComponent<Card>().cardType == CardType.Weapon ?
                        damagePassiveCard : cooldownPassiveCard;
                }
            }
        }

        selectionState = SelectionState.Waiting;

        yield return new WaitForSeconds(0.25f);

        if (numOfTraitors > 0)
        {
            StartCoroutine(ShowTraitorCanvas(FindAnyObjectByType<CardManager>().traitorType));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            selectionState = SelectionState.InGame;
            ResumeGameplay();
        }
    }

    #region Helper Functions

    public void SelectCard(GameObject card)
    {
        Debug.Log($"Card Selected: {card.name}");
        selectedCard = card;
        CardHandler cardHandler = card.GetComponent<CardHandler>();
        if (cardHandler) cardHandler.showPlayerIcon(PlayerManager.instance.playerSprites[currentPlayerIndex]);
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

    #endregion

    // Sets the value of final room
    public void SetIsFinalRoom(int num)
    {
        numOfTraitors = num;
    }

    public void ResumeGameplay()
    {
        FindAnyObjectByType<CardManager>().ResumeGameplay(selectedCards, cardList);
    }

    float GetRarityWeight(CardRarity rarity, float completion)
    {
        if (completion < 0.25f) // Early Game (First Quarter)
        {
            float t = completion * 4f;

            switch (rarity)
            {
                case CardRarity.Common:
                    return Mathf.Lerp(0.50f, 0.35f, t);
                case CardRarity.Uncommon:
                    return Mathf.Lerp(0.40f, 0.35f, t);
                case CardRarity.Rare:
                    return Mathf.Lerp(0.10f, 0.30f, t);
                default:
                    return 0f;
            }
        }
        else // Late Game
        {
            float t = (completion - 0.25f) * 4f / 3f;

            switch (rarity)
            {
                case CardRarity.Common:
                    return Mathf.Lerp(0.30f, 0.00f, t);
                case CardRarity.Uncommon:
                    return Mathf.Lerp(0.35f, 0.10f, t);
                case CardRarity.Rare:
                    return Mathf.Lerp(0.30f, 0.15f, t);
                case CardRarity.Epic:
                    return Mathf.Lerp(0.05f, 0.35f, t);
                case CardRarity.Legendary:
                    return Mathf.Lerp(0.00f, 0.40f, t);
                default:
                    return 0f;
            }
        }
    }
    Card GetWeightedRandomCard(GameObject[] cardPool, float completion)
    {
        float totalWeight = 0f;
        List<float> weights = new();

        foreach (GameObject cardObj in cardPool)
        {
            Card card = cardObj.GetComponent<Card>();
            float weight = GetRarityWeight(card.cardRarity, completion);
            weights.Add(weight);
            totalWeight += weight;
        }

        float rand = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < cardPool.Length; i++)
        {
            cumulative += weights[i];
            if (rand <= cumulative)
            {
                return cardPool[i].GetComponent<Card>();
            }
        }

        return cardPool[0].GetComponent<Card>(); // fallback
    }

    #endregion

    #region IEnums

    private IEnumerator ShowTraitorCanvas(BaseTraitor traitorType)
    {
        PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();

        foreach (PlayerData playerData in players)
        {
            if (!playerData.isJoined) continue;
            playerData.playerInput.SwitchCurrentActionMap("Locked");
        }

        GameObject traitorCanvas = Instantiate(traitorCanvasPrefab, null);
        traitorCanvas.GetComponent<TraitorCanvasManager>().SetTraitorType(traitorType);

        yield return new WaitForSeconds(3f);

        selectionState = SelectionState.TraitorConfirming;

        foreach (PlayerData playerData in players)
        {
            if (!playerData.isJoined) continue;
            playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");
        }

        traitorCanvas.GetComponent<TraitorCanvasManager>().StartReadyCheck(players);
    }

    #endregion
}
