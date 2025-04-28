// AUTHOR: Julian
// Tracks player actions to be displayed on the HUD and in the game summary

using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] public ScoreStats scoreStats;
    public ScoreStats _ScoreStats {get{return scoreStats;}}
    public float timeStartedRoom;


    public float GetScore() {
        return scoreStats.score;
    }

    public void AddScore(int additionalScore) {
        scoreStats.score += additionalScore;
        if (scoreStats.score < 0)
        {
            scoreStats.score = 0;
        }
        GetComponent<PlayerHUD>().UpdateScoreText(scoreStats.score);
    }

    public void AddDamageDealt(float additionalDamage) {
        scoreStats.damageDealt += additionalDamage;
        AddScore((int) additionalDamage * 10);
    }

    public void AddDamageTaken(float additionalDamage) {
        scoreStats.damageTaken += additionalDamage;
    }

    public void AddHealing(float healing) {
        scoreStats.healingGiven += healing;
        AddScore((int) healing * 15);
    }

    public void IncrementKills() {
        scoreStats.kills++;
        AddScore(100);
    }

    public void IncrementDeaths() {
        scoreStats.deaths++;
        AddScore(-1000);
    }

    public void IncrementProjectilesHit() {
        scoreStats.projectilesHit++;
    }

    public void IncrementProjectilesShot() {
        scoreStats.projectilesShot++;
    }

    public void IncrementHazardsRanInto() {
        scoreStats.hazardsRanInto++;
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

    public void IncrementWeaponActivated() {
        scoreStats.weaponActivated++;
    }

    public void IncrementSecondaryActivated() {
        scoreStats.secondaryActivated++;
    }

    public void SetTimeStarted()
    {
        timeStartedRoom = Time.time;
    }

    public void AddTimeAlive(float currentTime)
    {
        scoreStats.timeAlive += (currentTime - timeStartedRoom);
    }

    public void SetWonGame()
    {
        scoreStats.wonGame = true;
    }

    public void SetTraitor()
    {
        scoreStats.isTraitor = true;
    }
}

[Serializable] public struct ScoreStats {
    public float damageDealt, damageTaken, healingGiven, timeAlive;
    public int score, kills, deaths, weaponsPicked, secondariesPicked, passivesPicked, weaponActivated, secondaryActivated, projectilesHit, projectilesShot, hazardsRanInto;
    public bool isTraitor, wonGame;
}
