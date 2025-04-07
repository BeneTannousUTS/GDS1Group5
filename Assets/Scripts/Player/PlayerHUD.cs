// AUTHOR: BENEDICT
// This script initialises HUD elements for each player that is joined into a game

using System;
using System.Collections;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    int playerNum;

    GameObject hud;
    UIComponentHelper helper;
    
    float timeSincePrimary;
    float primaryCooldown;
    bool shouldUpdatePrimary = false;
    
    float timeSinceSecondary;
    float secondaryCooldown;
    bool shouldUpdateSecondary = false;

    private void Start()
    {
    }

    // Called every frame
    private void Update()
    {
        UpdateCooldowns();
    }

    // Set the player number text in the HUD panel
    public void SetPlayerNum(int playerIndex)
    {
        EnsureHUD();
        playerNum = playerIndex + 1;
        helper.playerNumTMP.text = "P" + playerNum;
    }
    
    // Set the player colour in the HUD panel
    public void SetHUDColour(Color healthColour)
    {
        EnsureHUD(); 
        if (helper != null)
        {
            helper.playerNumTMP.color = healthColour;
            helper.healthSlider.color = healthColour;
        }
    }

    // Set the Healthbar's fill amount and text
    public void SetHealthbarDetails(float currentHealth, float maxHealth)
    {
        helper.healthSlider.fillAmount = (currentHealth / maxHealth);
        helper.healthTextOver.text = currentHealth + "/" + maxHealth;
        helper.healthTextUnder.text = currentHealth + "/" + maxHealth;
    }

    // Start cooldown animation for primary ability
    public void StartPrimaryCooldownAnim(float cooldown)
    {
        timeSincePrimary = 0;
        shouldUpdatePrimary = true;
        primaryCooldown = cooldown;
    }
    
    // Start cooldown animation for secondary ability
    public void StartSecondaryCooldownAnim(float cooldown)
    {
        timeSinceSecondary = 0;
        shouldUpdateSecondary = true;
        secondaryCooldown = cooldown;
    }

    // Check which cooldown panels need to be updated and update accordingly
    void UpdateCooldowns()
    {
        if (shouldUpdatePrimary)
        {
            timeSincePrimary += Time.deltaTime;
            helper.primaryAbility.fillAmount = timeSincePrimary / primaryCooldown;
            if (timeSincePrimary >= primaryCooldown)
            {
                shouldUpdatePrimary = false;
            }
        }

        if (shouldUpdateSecondary)
        {
            timeSinceSecondary += Time.deltaTime;
            helper.secondaryAbility.fillAmount = timeSinceSecondary / secondaryCooldown;
            if (timeSinceSecondary >= secondaryCooldown)
            {
                shouldUpdateSecondary = false;
            }
        }
    }

    public void SetPrimarySprite(Sprite sprite)
    {
        helper.primaryAbility.sprite = sprite;
        helper.primaryAbilityBackground.sprite = sprite;
    }

    public void SetSecondarySprite(Sprite sprite)
    {
        helper.secondaryAbility.sprite = sprite;
        helper.secondaryAbilityBackground.sprite = sprite;
    }

    // Make sure the HUD exists before setting values
    void EnsureHUD()
    {
        if (hud == null)
        {
            hud = Instantiate(playerHUDPrefab, GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform, false);
            helper = hud.GetComponentInChildren<UIComponentHelper>();
        }
    }

    public void UpdateStatsDisplay()
    {
        helper.statsHelper.UpdateDamageText(GetComponent<PlayerStats>().GetStrengthStat());
        helper.statsHelper.UpdateSpeedText(GetComponent<PlayerStats>().GetMoveStat());
        helper.statsHelper.UpdateHealthText(GetComponent<PlayerStats>().GetHealthStat());
    }

    public void SetStatsAlpha(int alpha)
    {
        helper.statsHelper.UpdateDamageTextAlpha(alpha);
        helper.statsHelper.UpdateSpeedTextAlpha(alpha);
    }
}
