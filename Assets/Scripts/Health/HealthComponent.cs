// AUTHOR: Alistair
// Handles storing a player/enemy's health and checking whether death has occured

using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    public float invicibilityFrameTime;
    private bool invincible = false;
    [SerializeField] private float currentHealth;
    private bool isDead = false;
    private AudioManager audioManager;

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

    IEnumerator DamageFlash() 
    {
        Color baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;
    }

    IEnumerator HealingFlash() 
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
        yield return new WaitForSeconds(0.5f);
        if (gameObject.CompareTag("Player") == false) {
            Destroy(gameObject);
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    // Takes damage equal to the damageValue then checks if is dead
    public void TakeDamage(float damageValue) 
    {
        if (damageValue < 0f) 
        {
            currentHealth -= damageValue;
            
            StartCoroutine(HealingFlash());

            if (currentHealth >= maxHealth) 
            {
                currentHealth = maxHealth;
            }
            
            // Call HUD component function to update healthbar if player
            if (gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
            }
        }

        else if (invincible == false) 
        {
            currentHealth -= damageValue;

        // this is a really scuffed way to determine if it is a player or not but it works
        string audioDamageType = GetComponent<PlayerAttack>() ? "PlayerDamage" : "EnemyDamage"; 
        audioManager.PlaySoundEffect(audioDamageType);

            StartCoroutine(DamageFlash());
            StartCoroutine(DoInvincibilityFrames(invicibilityFrameTime));
            
            // Call HUD component function to update healthbar if player
            if (gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
            }

            if (currentHealth <= 0f) 
            {
                currentHealth = 0f;
                StartCoroutine(Die());
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
}
