// AUTHOR: Julian
// Handles taking each player's score and displaying it in the results scene

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using TMPro;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Linq;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private List<ScoreStats> playerScores = new List<ScoreStats>();
    [SerializeField] private Sprite[] playerIcons;

    [SerializeField] private GameObject playerBoxPrefab;
    private GameObject resultsBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded() {
        resultsBox = GameObject.Find("Canvas/Main Menu/ResultsBox");
        ShowResults();
    }

    void ShowResults() {
        for (int player = 0; player < playerScores.Count; player++) {
            GameObject playerBox = Instantiate(playerBoxPrefab, Vector3.zero, quaternion.identity, resultsBox.transform);
            playerBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player " + (player + 1);
            playerBox.transform.GetChild(1).GetComponent<Image>().sprite = playerIcons[player];
            playerBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score: " + playerScores[player].score;
        }
    }

    public void GetPlayerScores() {
        List<GameObject> players = FindAnyObjectByType<GameManager>().GetPlayerList();

        foreach (GameObject player in players) {
            ScoreStats s = player.GetComponent<PlayerScore>()._ScoreStats;
            ScoreStats score = new ScoreStats(s.score, s.damageDealt, s.damageTaken, s.healingGiven, s.kills, s.deaths, s.weaponsPicked, s.secondariesPicked, s.passivesPicked);
            //score = player.GetComponent<PlayerScore>();
            playerScores.Add(score);
            Debug.Log("FOUND PLAYER");
        }
    }
}
