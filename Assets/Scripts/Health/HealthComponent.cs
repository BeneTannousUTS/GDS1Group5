// AUTHOR: Alistair
// Handles storing a player/enemy's health and checking whether death has occured

using UnityEngine;
using System.Collections;
using Unity.Mathematics;

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
    private MaterialPropertyBlock materialPropertyBlock; // Used so the damage flash only affects this object

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
        GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
        gameObject.GetComponent<Animator>().SetTrigger("revived");

    }

    IEnumerator DamageFlash() 
    {
        /*Color baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;*/
        float currentFlash = 0f;
        float lerpTime = 0f;
        while (lerpTime < flashDuration) {
            lerpTime += Time.deltaTime;
            currentFlash = Mathf.Lerp(2f, 0f, lerpTime/flashDuration);
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("flashAmount", currentFlash);
            yield return null;
        }
    }

    IEnumerator HealingFlash() 
    {
        /*Color baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;*/
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
        if (gameObject.CompareTag("Player") == false) {
            Destroy(gameObject);
        }
        else {
            //gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            Debug.Log("Die");
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckGameState();
            gameObject.GetComponent<Animator>().SetTrigger("dead");
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
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Traitor"))
            {
                if (gameObject.GetComponent<PlayerHUD>() != null)
                {
                    GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
                }
            }
            
            GetComponent<SmallHealthBar>().SetHealthBarFill(currentHealth/maxHealth);
            GetComponent<SmallHealthBar>().SetHealthBarFill(currentHealth / maxHealth);
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
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Traitor"))
            {
                if (gameObject.GetComponent<PlayerHUD>() != null)
                {
                    gameObject.GetComponent<PlayerHUD>().SetHealthbarDetails(currentHealth, maxHealth);
                }
            }
            
            GetComponent<SmallHealthBar>().SetHealthBarFill(currentHealth/maxHealth);

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
