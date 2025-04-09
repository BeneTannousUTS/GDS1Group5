// AUTHOR: Julian
// Tracks player actions to be displayed on the HUD and in the game summary

using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    [SerializeField] public ScoreStats scoreStats;

    public ScoreStats _ScoreStats {get{return scoreStats;}}
    public float GetScore() {
        return scoreStats.score;
    }

    public void AddScore(float additionalScore) {
        scoreStats.score += additionalScore;
        GetComponent<PlayerHUD>().UpdateScoreText(scoreStats.score);
    }

    public void AddDamageDealt(float additionalDamage) {
        scoreStats.damageDealt += additionalDamage;
        AddScore(additionalDamage*10);
    }

    public void AddDamageTaken(float additionalDamage) {
        scoreStats.damageTaken += additionalDamage;
    }

    public void AddHealing(float healing) {
        scoreStats.healingGiven += healing;
        AddScore(healing*15);
    }

    public void IncrementKills() {
        scoreStats.kills++;
        AddScore(100);
    }

    public void IncrementDeaths() {
        scoreStats.deaths++;
        AddScore(-1000);
    }

    public void IncrementWeaponsPicked() {
        scoreStats.weaponsPicked++;
    }

    public void IncrementSecondariesPicked() {
        scoreStats.secondariesPicked++;
    }

    public void IncrementPassivesPicked() {
        scoreStats.passivesPicked++;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable] public struct ScoreStats {
    public float score, damageDealt, damageTaken, healingGiven;
    public int kills, deaths, weaponsPicked, secondariesPicked, passivesPicked;

    public ScoreStats(float SCORE, float DAMAGEDEALT, float DAMAGETAKEN, float HEALINGGIVEN, int KILLS, int DEATHS, int WEAPONSPICKED, int SECONDARIESPICKED, int PASSIVESPICKED) {
        score = SCORE;
        damageDealt = DAMAGEDEALT;
        damageTaken = DAMAGETAKEN;
        healingGiven = HEALINGGIVEN;
        kills = KILLS;
        deaths = DEATHS;
        weaponsPicked = WEAPONSPICKED;
        secondariesPicked = SECONDARIESPICKED;
        passivesPicked = PASSIVESPICKED; 
    }
}
