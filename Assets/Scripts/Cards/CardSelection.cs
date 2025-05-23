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
    Rare,
    Legendary,
    Traitor
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
    [SerializeField]
    GameObject passiveConfirmPrefab;
    [SerializeField]
    GameObject traitorConfirmPrefab;
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
    GameObject[] healthCards;
    public SelectionState selectionState = SelectionState.InGame;
    [SerializeField]
    GameObject cardOutlinePrefab;
    private int traitorIndex = -1;

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
        float dungeonCompletionPercent = (float)FindAnyObjectByType<DungeonManager>().GetRoomCount() / (float)FindAnyObjectByType<DungeonManager>().GetDungeonLength();

        GameObject weapon = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Weapon
            && card.GetComponent<Card>().cardRarity == GetWeightedCardRarity(cards, dungeonCompletionPercent))
            .FirstOrDefault();

        GameObject secondary = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Secondary
            && card.GetComponent<Card>().cardRarity == GetWeightedCardRarity(cards, dungeonCompletionPercent))
            .FirstOrDefault();

        GameObject passive = cards
            .Where(card => card.GetComponent<Card>().cardType == CardType.Passive
            && card.GetComponent<Card>().cardRarity == GetWeightedCardRarity(cards, dungeonCompletionPercent))
            .FirstOrDefault();

        if (roomNum == 0)
        {
            for (int j = 0; j < 4; ++j)
            {
                CardRarity randomRarirty = GetWeightedCardRarity(cards, dungeonCompletionPercent);

                List<GameObject> remainingCards = cards
                .Where(card => card.GetComponent<Card>().cardType == CardType.Weapon
                && card.GetComponent<Card>().cardRarity == randomRarirty)
                .ToList();

                tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
            }
        }
        else if (roomNum == 1)
        {
            for (int j = 0; j < 4; ++j)
            {
                CardRarity randomRarirty = GetWeightedCardRarity(cards, dungeonCompletionPercent);

                List<GameObject> remainingCards = cards
                .Where(card => card.GetComponent<Card>().cardType == CardType.Secondary
                && card.GetComponent<Card>().cardRarity == randomRarirty)
                .ToList();

                tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
            }
        }
        else
        {
            if (weapon != null) tempCardList.Add(weapon);
            if (passive != null) tempCardList.Add(passive);
            if (secondary != null) tempCardList.Add(secondary);

            CardRarity randomRarirty = GetWeightedCardRarity(cards, dungeonCompletionPercent);

            while (tempCardList.Count < 4)
            {
                List<GameObject> remainingCards = cards
                .Where(card => card.GetComponent<Card>().cardRarity == randomRarirty)
                .Except(tempCardList).ToList();

                tempCardList.Add(remainingCards[Random.Range(0, remainingCards.Count)]);
            }
        }

        tempCardList = tempCardList.OrderBy(x => Random.value).ToList();

        int i = 0;
        foreach (GameObject card in tempCardList)
        {
            GameObject cardObj = Instantiate(card, transform);

            GameObject cardOutline = Instantiate(cardOutlinePrefab, cardObj.transform);
            cardOutline.transform.SetSiblingIndex(0);

            cardOutline.GetComponent<Image>().color = GetColourFromRarity(cardObj.GetComponent<Card>().cardRarity) * new Vector4(1f, 1f, 1f, 0.25f);

            cardList[i++] = cardObj;
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

            if (playerSelectionPos == 3)
            {
                foreach (GameObject card in cardList)
                {
                    if (card.GetComponent<Button>() != null)
                    {
                        card.GetComponent<Button>().Select();
                        card.GetComponent<CardHandler>().OnSelect(null);

                        SelectCard(card);

                        Destroy(selectedCard.GetComponent<Button>());
                        selectedCard.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
                        selectedCard.GetComponent<CardHandler>().OnDeselect(null);

                        selectedCard = null;

                        break;
                    }
                }
                break;
            }

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
                CardRarity ranCardRarity = cardList[newIndex].GetComponent<Card>().cardRarity;

                GameObject[] cardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == ranCardType
                    && card.GetComponent<Card>().cardRarity == ranCardRarity)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(cardsOfSameType, dungeonCompletionPercent);
                cardList[newIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
                continue;
            }

            CardType cardType = cardList[cardIndex].GetComponent<Card>().cardType;
            CardRarity cardRarity = cardList[cardIndex].GetComponent<Card>().cardRarity;
            if (cardType == CardType.Weapon)
            {
                GameObject filterWeapon = players[playerIndex].playerInput.gameObject.GetComponent<PlayerAttack>().currentWeapon;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterWeapon
                    && card.GetComponent<Card>().cardRarity == cardRarity)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(filteredCardsOfSameType, dungeonCompletionPercent);
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else if (cardType == CardType.Secondary)
            {
                GameObject filterSecondary = players[playerIndex].playerInput.gameObject.GetComponent<PlayerSecondary>().currentSecondary;
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().abilityObject != filterSecondary
                    && card.GetComponent<Card>().cardRarity == cardRarity)
                    .ToArray();
                Card replacementCard = GetWeightedRandomCard(filteredCardsOfSameType, dungeonCompletionPercent);
                cardList[cardIndex].GetComponent<CardHandler>().ReplaceCard(replacementCard);
            }
            else
            {
                GameObject[] filteredCardsOfSameType = cards
                    .Where(card => card.GetComponent<Card>().cardType == cardType
                    && card.GetComponent<Card>().cardRarity == cardRarity)
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
                List<PlayerData> joinedPlayers = new List<PlayerData>();

                foreach (PlayerData player in players)
                {
                    if (player.isJoined) joinedPlayers.Add(player);
                }

                for (int i = 0; i < numOfTraitors; ++i)
                {
                    traitorIndex = joinedPlayers[Random.Range(0, joinedPlayers.Count())].playerIndex;

                    Debug.Log($"Traitor Index: {traitorIndex}");

                    cardList[selectedCards[traitorIndex]].GetComponent<CardHandler>().setTraitorCard(traitorCardSprite);
                }
            }

            FindAnyObjectByType<AudioManager>().PlayTraitorTheme();
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
        List<BaseConfirmHandler> handlers = new List<BaseConfirmHandler>(); // at the player's index will hold the confirm card

        if (roomNum > 1)
        {

            PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();

            coverCanvas.SetActive(true);
            selectionState = SelectionState.ConfirmingSwap;

            foreach (PlayerData playerData in players)
            {
                if (!playerData.isJoined) continue;

                CardType cardType = cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardType;

                if (cardType == CardType.Weapon || cardType == CardType.Secondary)
                {
                    GameObject confirmCard = Instantiate(confirmCardPrefab, confirmCanvas.transform);
                    playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");

                    ConfirmCardHandler confirmCardHandler = confirmCard.GetComponent<ConfirmCardHandler>();
                    confirmCardHandler.init(playerData.playerInput);

                    confirmCardHandler.SetupCard(
                        $"Player {playerData.playerIndex + 1}",
                        playerData.playerColour,
                        PlayerManager.instance.playerSprites[playerData.playerIndex],
                        cardType == CardType.Weapon ?
                            playerData.playerInput.GetComponent<PlayerHUD>().GetUIComponentHelper().primaryAbility.sprite :
                            playerData.playerInput.GetComponent<PlayerHUD>().GetUIComponentHelper().secondaryAbility.sprite,
                        cardList[selectedCards[playerData.playerIndex]].GetComponent<Image>().sprite
                    );

                    handlers.Add(confirmCardHandler);
                }
                else if (cardType == CardType.Passive && cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardRarity != CardRarity.Traitor)
                {
                    GameObject confirmCard = Instantiate(passiveConfirmPrefab, confirmCanvas.transform);

                    PassiveConfirmHandler confirmCardHandler = confirmCard.GetComponent<PassiveConfirmHandler>();

                    GameObject selectedPassiveObj = cardList[selectedCards[playerData.playerIndex]];

                    float currentPassiveMod = 1f;
                    float passiveModDiff = 0.0f;

                    PassiveStats passiveStats = selectedPassiveObj.GetComponent<Card>().abilityObject.GetComponent<PassiveStats>();
                    PlayerStats playerStats = playerData.playerInput.GetComponent<PlayerStats>();

                    if (passiveStats.healthMod > 0)
                    {
                        currentPassiveMod = playerStats.GetHealthStat();
                        passiveModDiff = passiveStats.GetHealthMod();

                        confirmCardHandler.SetupCard(
                            $"Player {playerData.playerIndex + 1}",
                            playerData.playerColour,
                            PlayerManager.instance.playerSprites[playerData.playerIndex],
                            selectedPassiveObj.GetComponent<Card>().cardName,
                            selectedPassiveObj.GetComponent<Image>().sprite,
                            $"{currentPassiveMod}",
                            $"{currentPassiveMod + passiveModDiff}"
                        );
                    }
                    else
                    {
                        if (passiveStats.strengthMod > 0)
                        {
                            currentPassiveMod = playerStats.GetStrengthStat();
                            passiveModDiff = passiveStats.GetStrengthMod() / 100f;
                        }
                        else if (passiveStats.moveMod > 0)
                        {
                            currentPassiveMod = playerStats.GetMoveStat();
                            passiveModDiff = passiveStats.GetMoveMod() / 100f;
                        }
                        else if (passiveStats.cooldownMultiplier > 0)
                        {
                            currentPassiveMod = playerStats.GetCooldownStat();
                            passiveModDiff = passiveStats.GetCooldownMod() / 100f;
                        }
                        else if (passiveStats.lifestealMulitplier > 0)
                        {
                            currentPassiveMod = playerStats.GetLifestealStat();
                            passiveModDiff = passiveStats.GetLifestealMod() / 100f;
                        }
                        else if (passiveStats.knockbackMult > 0)
                        {
                            currentPassiveMod = playerStats.GetKnockbackStat();
                            passiveModDiff = passiveStats.GetKnockbackMod() / 100f;
                        }

                        confirmCardHandler.SetupCard(
                            $"Player {playerData.playerIndex + 1}",
                            playerData.playerColour,
                            PlayerManager.instance.playerSprites[playerData.playerIndex],
                            selectedPassiveObj.GetComponent<Card>().cardName,
                            selectedPassiveObj.GetComponent<Image>().sprite,
                            $"x{currentPassiveMod}",
                            $"x{currentPassiveMod + passiveModDiff}"
                        );
                    }
                }
                else if (cardType == CardType.Passive && cardList[selectedCards[playerData.playerIndex]].GetComponent<Card>().cardRarity == CardRarity.Traitor)
                {
                    GameObject confirmCard = Instantiate(traitorConfirmPrefab, confirmCanvas.transform);

                    PassiveConfirmHandler confirmCardHandler = confirmCard.GetComponent<PassiveConfirmHandler>();

                    confirmCardHandler.SetupCard(
                        $"Player {playerData.playerIndex + 1}",
                        playerData.playerColour,
                        PlayerManager.instance.playerSprites[playerData.playerIndex]);
                }
            }

            yield return ConfirmManager.Instance.WaitForAllConfirmations(handlers);

            foreach (BaseConfirmHandler cardHandler in handlers)
            {
                if (cardHandler.confirmedChoice == false)
                {
                    Debug.Log($"Int: {(int)cardList[selectedCards[cardHandler.playerIndex]].GetComponent<Card>().cardRarity}");
                    cardList[selectedCards[cardHandler.playerIndex]] = healthCards[(int)cardList[selectedCards[cardHandler.playerIndex]].GetComponent<Card>().cardRarity];
                }
            }

            selectionState = SelectionState.Waiting;
            
            if (handlers.Count() == 0)
            {
                yield return new WaitForSeconds(3.0f);
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
        if (completion < 0.2f) // Early Game (0 - 20% completion)
        {
            float t = completion * 4f;

            switch (rarity)
            {
                case CardRarity.Common:
                    return Mathf.Lerp(1.0f, 0.40f, t);
                case CardRarity.Rare:
                    return Mathf.Lerp(0.00f, 0.60f, t);
                default:
                    return 0f;
            }
        }
        else if (completion >= 0.2f && completion < 0.70f) // Mid Game (20% to 70% completion)
        {
            float t = (completion - 0.25f) * 4f / 3f;

            switch (rarity)
            {
                case CardRarity.Common:
                    return Mathf.Lerp(0.40f, 0.0f, t);
                case CardRarity.Rare:
                    return Mathf.Lerp(0.60f, 0.40f, t);
                case CardRarity.Legendary:
                    return Mathf.Lerp(0.00f, 0.50f, t);
                default:
                    return 0f;
            }
        }
        else // Late Game (70% - 100% completion)
        {
            switch (rarity)
            {
                case CardRarity.Rare:
                    return 0.30f;
                case CardRarity.Legendary:
                    return 0.70f;
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

    CardRarity GetWeightedCardRarity(GameObject[] cardPool, float completion)
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
                return cardPool[i].GetComponent<Card>().cardRarity;
            }
        }

        return CardRarity.Common; // fallback
    }

    public Color GetColourFromRarity(CardRarity rarity)
    {
        switch (rarity)
        {
            case CardRarity.Rare:
                return new Vector4(0f, 0.9f, 1f, 1f);
            case CardRarity.Legendary:
                return new Vector4(1f, 0.85f, 0f, 1f);
            case CardRarity.Traitor:
                return Color.red;
            default:
                return Color.white;
        }
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
        traitorCanvas.GetComponent<TraitorCanvasManager>().SetTraitor($"Player {traitorIndex + 1}",
        FindAnyObjectByType<GameSceneManager>().playerColours[traitorIndex]);
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
