// AUTHOR: Alistair
// Handles storing a player/enemy's health and checking whether death has occured

using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using System;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    public float invicibilityFrameTime;
    private bool invincible = false;
    [SerializeField] private float currentHealth;
    private bool isDead = false;
    private AudioManager audioManager;
    [SerializeField] private GameObject healParticles;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private bool isObject = false;

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetMaxHealth(float health)
    {
        if (maxHealth < health)
        {
            currentHealth += health - maxHealth;
            maxHealth = health;
        }
    }

    //Brings back player back to life with half there max health
    public void Revive()
    {
        isDead = false;
        currentHealth = maxHealth / 2;
        UpdateHUDHealthBar();
        FindAnyObjectByType<PopupManager>().SpawnSmallPopup(gameObject, "Revived!", PlayerManager.instance.players[gameObject.GetComponent<PlayerIndex>().playerIndex].playerColour);
        gameObject.GetComponent<Animator>().SetTrigger("revived");
    }

    IEnumerator DamageFlash()
    {
        float currentFlash = 0f;
        float lerpTime = 0f;
        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            currentFlash = Mathf.Lerp(2f, 0f, lerpTime / flashDuration);
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("flashAmount", currentFlash);
            yield return null;
        }
    }

    IEnumerator HealingFlash()
    {
        GameObject healParticle = Instantiate(healParticles, gameObject.transform);
        //healParticle.transform.position = gameObject.transform.position;
        yield return null;
    }

    IEnumerator DoInvincibilityFrames(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        yield return new WaitForSeconds(0.05f);
        if (gameObject.CompareTag("Traitor") == true)
        {
            gameObject.GetComponent<BaseTraitor>().LoseCondition();
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckGameState();
        }
        else if (gameObject.CompareTag("Destructible"))
        {
            gameObject.GetComponent<Destructible>().SpawnItems();
            AudioManager.instance.PlaySoundEffect("PotsBreaking");
        }
        else if (gameObject.CompareTag("Player") == false)
        {
            Destroy(gameObject);
        }
        else
        {
            if (gameObject.GetComponent<Polymorph>() != null)
            {
                gameObject.GetComponent<Polymorph>().RemoveMorph();
            }
            //gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            Debug.Log("Die");
            FindAnyObjectByType<PopupManager>().SpawnSmallPopup(gameObject, "You Died!", PlayerManager.instance.players[gameObject.GetComponent<PlayerIndex>().playerIndex].playerColour);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckGameState();
            gameObject.GetComponent<Animator>().SetTrigger("dead");
            gameObject.GetComponent<PlayerScore>().IncrementDeaths();
            gameObject.GetComponent<PlayerScore>().AddTimeAlive(Time.time);
            gameObject.GetComponent<VibrationManager>().StopAllVibrations(gameObject.GetComponent<PlayerInput>().GetDevice<Gamepad>());
        }
    }

    // Takes damage equal to the damageValue then checks if is dead
    public void TakeDamage(float damageValue)
    {
        if (damageValue != 0)
        {
            if (damageValue < 0f)
            {
                damageValue = Math.Max(1f, damageValue);
                currentHealth = Math.Max(0, currentHealth - damageValue);

                if (!isObject)
                {
                    
                    if (damageValue > -1000f)
                    {
                        if (damageValue < -3f) AudioManager.instance.PlaySoundEffect("UIConfirm", 1.5f);
                        FindAnyObjectByType<PopupManager>().SpawnSmallPopup(gameObject, $"+ {(int)(damageValue * -1f)}", Color.green, damageValue / -10);
                    }
                }

                StartCoroutine(HealingFlash());

                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                }

                // Call HUD component function to update healthbar if player
                if (gameObject.CompareTag("Player") || gameObject.CompareTag("Traitor"))
                {
                    if (gameObject.GetComponent<PlayerHUD>() != null)
                    {
                        UpdateHUDHealthBar();
                    }

                    if (gameObject.GetComponent<VibrationManager>() != null)
                    {
                        gameObject.GetComponent<VibrationManager>().StartVibrationPattern(GetComponent<PlayerInput>().GetDevice<Gamepad>(),
                            VibrationManager.VibrationPattern.HealPattern);
                    }
                } else if (gameObject.GetComponent<Destructible>() != null)
                {
                    // add broke pot to player score
                }

                if (GetComponent<SmallHealthBar>() != null)
                {
                    GetComponent<SmallHealthBar>().SetHealthBarFill(currentHealth / maxHealth);
                }
            }

            else if (invincible == false)
            {
                currentHealth = Math.Max(0, currentHealth - damageValue);
                if (!isObject)
                {
                    FindAnyObjectByType<PopupManager>().SpawnSmallPopup(gameObject, $"- {(int)(damageValue)}", Color.red, damageValue / 10f);
                }

                // determining which audio effect should player
                if (GetComponent<PlayerAttack>()) audioManager.PlaySoundEffect("PlayerDamage", UnityEngine.Random.Range(0.9f, 1.1f));
                if (GetComponent<EnemyAttack>()) audioManager.PlaySoundEffect("EnemyDamage", UnityEngine.Random.Range(0.9f, 1.1f));

                StartCoroutine(DamageFlash());
                if (invicibilityFrameTime != 0f) {
                    StartCoroutine(DoInvincibilityFrames(invicibilityFrameTime));
                }

                // Call HUD component function to update healthbar if player
                if (gameObject.CompareTag("Player") || gameObject.CompareTag("Traitor"))
                {
                    GetComponent<PlayerScore>().AddDamageTaken(damageValue);
                    if (gameObject.GetComponent<PlayerHUD>() != null)
                    {
                        UpdateHUDHealthBar();
                        gameObject.GetComponent<PlayerHUD>().GetUIComponentHelper().healthBarPanel.GetComponent<HUDElementShakeHandler>().ShakeCard();
                    }

                    if (gameObject.GetComponent<VibrationManager>() != null)
                    {
                        gameObject.GetComponent<VibrationManager>().StartVibrationPattern(GetComponent<PlayerInput>().GetDevice<Gamepad>(),
                            VibrationManager.VibrationPattern.HealPattern);
                    }
                }

                if (GetComponent<SmallHealthBar>() != null)
                {
                    GetComponent<SmallHealthBar>().SetHealthBarFill(currentHealth / maxHealth);
                }

                if (currentHealth <= 0f)
                {
                    currentHealth = 0f;
                    StartCoroutine(Die());
                }
            }
        }
    }

    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
    }

    // Sets currentHealth to maxHealth & gets audio manager by tag
    void Start()
    {
        currentHealth = maxHealth;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void UpdateHUDHealthBar()
    {
        GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    //Used when removing all passives a player has gained with debug
    public void ResetPlayerHealth()
    {
        maxHealth = 100;
        currentHealth = 100;
    }

    public void ToggleInvincible()
    {
        invincible = !invincible;
    }
}
