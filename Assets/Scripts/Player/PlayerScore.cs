// AUTHOR: Julian
// Tracks player actions to be displayed on the HUD and in the game summary

using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    [SerializeField] private float score, damageDealt, damageTaken, healingGiven;
    [SerializeField] private int kills, deaths, weaponsPicked, secondariesPicked, passivesPicked;


    public void AddScore(float additionalScore) {
        score += additionalScore;
        GetComponent<PlayerHUD>().UpdateScoreText(score);
    }

    public void AddDamageDealt(float additionalDamage) {
        damageDealt += additionalDamage;
        AddScore(additionalDamage*10);
    }

    public void AddDamageTaken(float additionalDamage) {
        damageTaken += additionalDamage;
    }

    public void AddHealing(float healing) {
        healingGiven += healing;
        AddScore(healing*15);
    }

    public void IncrementKills() {
        kills++;
        AddScore(100);
    }

    public void IncrementDeaths() {
        deaths++;
        AddScore(-1000);
    }

    public void IncrementWeaponsPicked() {
        weaponsPicked++;
    }

    public void IncrementSecondariesPicked() {
        secondariesPicked++;
    }

    public void IncrementPassivesPicked() {
        passivesPicked++;
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
