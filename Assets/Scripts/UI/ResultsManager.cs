// AUTHOR: Julian
// Handles taking each player's score and displaying it in the results scene

using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private List<ScoreStats> playerScores = new List<ScoreStats>();
    [SerializeField] private Sprite[] playerIcons;

    [SerializeField] private GameObject playerBoxPrefab;
    [SerializeField] private GameObject leaderSlotPrefab;
    public bool didLose = false;
    private GameObject resultsBox;
    private GameObject leaderBox;
    private GameObject winText;
    private GameObject mainMenu;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded()
    {
        resultsBox = GameObject.Find("Canvas/WinnersBox");
        leaderBox = GameObject.Find("Canvas/LeaderboardBox");
        winText = GameObject.Find("Canvas/WinText");
        mainMenu = GameObject.Find("Canvas/Main Menu");
        mainMenu.SetActive(false);

        if (didLose)
        {
            StartCoroutine(ShowLeaderboard());
        }
        else
        {
            StartCoroutine(ShowWinners());
        }
    }

    public void GetPlayerScores()
    {
        PlayerData[] players = FindAnyObjectByType<PlayerManager>().GetPlayers();

        foreach (PlayerData playerData in players)
        {
            if (!playerData.isJoined) continue;

            playerScores.Add(playerData.playerInput.gameObject.GetComponent<PlayerScore>()._ScoreStats);
        }
    }

    IEnumerator ShowWinners()
    {
        for (int playerIndex = 0; playerIndex < playerScores.Count; playerIndex++)
        {
            if (!playerScores[playerIndex].wonGame) continue;

            GameObject playerBox = Instantiate(playerBoxPrefab, Vector3.zero, quaternion.identity, resultsBox.transform);
            playerBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player " + (playerIndex + 1);
            Debug.Log($"Color: {PlayerManager.instance.players[playerIndex].playerColour}");
            playerBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = PlayerManager.instance.players[playerIndex].playerColour;
            playerBox.transform.GetChild(1).GetComponent<Image>().sprite = playerIcons[playerIndex];
            playerBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score: " + playerScores[playerIndex].score;

            if (playerScores[playerIndex].isTraitor)
            {
                winText.GetComponent<TMP_Text>().text = "The Traitor Wins!";
            }
            else
            {
                winText.GetComponent<TMP_Text>().text = "The Players Win!";
            }
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(ShowLeaderboard());

    }

    IEnumerator ShowLeaderboard()
    {
        foreach (Transform child in resultsBox.transform)
        {
            Destroy(child.gameObject);
        }

        winText.GetComponent<TMP_Text>().text = "Leaderboard";

        List<IndexScoreStat> indexedScoreStats = new List<IndexScoreStat>();

        for (int playerIndex = 0; playerIndex < playerScores.Count; ++playerIndex)
        {
            IndexScoreStat indexScoreStat = new IndexScoreStat();
            indexScoreStat.playerIndex = playerIndex;
            indexScoreStat.scoreStat = playerScores[playerIndex];
            indexedScoreStats.Add(indexScoreStat);
        }

        List<IndexScoreStat> playersWithoutAccolade = new List<IndexScoreStat>(indexedScoreStats);
        Dictionary<int, Accolade> playerToAccolade = new Dictionary<int, Accolade>();

        List<Accolade> orderedAccolades = new List<Accolade>
        {
            new ComebackKingAccolade(),
            new DontMissAccolade(),
            new SlayerAccolade(),
            new PunchingBagAccolade(),
            new WeaponHoarderAccolade(),
            new PassivePeteAccolade(),
            new MedicAccolade(),
            new SurvivorAccolade(),
            new CasperAccolade(),
        };

        foreach (Accolade accolade in orderedAccolades)
        {
            int bestPlayerIndex = accolade.SelectBest(playersWithoutAccolade);

            if (bestPlayerIndex != -1)
            {
                playerToAccolade.Add(bestPlayerIndex, accolade);
                playersWithoutAccolade.RemoveAll(p => p.playerIndex == bestPlayerIndex);
            }
        }

        foreach (IndexScoreStat leftover in playersWithoutAccolade)
        {
            Accolade fallback = new BoringAccolade();
            fallback.SelectBest(new List<IndexScoreStat> { leftover });
            playerToAccolade.Add(leftover.playerIndex, fallback);
        }
        playersWithoutAccolade.Clear();

        List<IndexScoreStat> rankedPlayers = indexedScoreStats.OrderByDescending(p => p.scoreStat.wonGame).ThenByDescending(p => p.scoreStat.score).ToList();

        for (int rankedIndex = 0; rankedIndex < rankedPlayers.Count; rankedIndex++)
        {
            GameObject leaderboardSlot = Instantiate(leaderSlotPrefab, Vector3.zero, quaternion.identity, leaderBox.transform);
            LeaderSlotHandler leaderSlotHandler = leaderboardSlot.GetComponent<LeaderSlotHandler>();

            Color playerColour = PlayerManager.instance.players[rankedPlayers[rankedIndex].playerIndex].playerColour;
            playerColour.a = 0.8f;
            leaderSlotHandler.background.color = playerColour;
            leaderSlotHandler.winText.text = rankedPlayers[rankedIndex].scoreStat.wonGame ? "Won" : "Lost";
            leaderSlotHandler.rankingText.text = GetOrdinal(rankedIndex);
            leaderSlotHandler.playerIcon.sprite = playerIcons[rankedPlayers[rankedIndex].playerIndex];
            leaderSlotHandler.scoreText.text = $"Score: {rankedPlayers[rankedIndex].scoreStat.score}";

            playerToAccolade.TryGetValue(rankedPlayers[rankedIndex].playerIndex, out Accolade accolade);

            leaderSlotHandler.accoladeTitle.text = accolade.title;
            leaderSlotHandler.accoladeText.text = accolade.body;
        }

        yield return new WaitForSeconds(5f);

        mainMenu.SetActive(true);
    }

    string GetOrdinal(int number)
    {
        if (number <= 0) return "1st";
        if (number == 1) return "2nd";
        if (number == 2) return "3rd";
        if (number == 3) return "4th";

        return (number + 1) + "th";
    }
}
