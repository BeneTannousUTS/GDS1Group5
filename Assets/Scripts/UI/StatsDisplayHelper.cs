//AUTHOR: Benedict
/*This script passes values to the text mesh pro elements in the HUD to show how stats have been influenced
by passive cards*/

using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayHelper : MonoBehaviour
{
    //public TextMeshProUGUI[] healthMultText;
    public TextMeshProUGUI damageMultText;
    public TextMeshProUGUI speedMultText;
    public TextMeshProUGUI knockbackMultText;
    public TextMeshProUGUI lifestealMultText;
    public TextMeshProUGUI cooldownMultText;
    
    [Space]
    public Image statsPanel;

    //Set this in the editor to adjust how long it will take to drop down in seconds
    public float statsPanelFillTime = 0.5f;
    
    public float statsPanelFillPercent = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // //Set the health modifier text
    // public void UpdateHealthText(float healthMod)
    // {
    //     foreach (TextMeshProUGUI text in healthMultText)
    //     {
    //         text.text = "(" + (healthMod/100).ToString("0.0") + "x)";
    //     }
    // }
    
    //Set the speed modifier text
    public void UpdateSpeedText(float speedMod)
    {
        speedMultText.text = speedMod.ToString("0.0");
    }
    
    //Set the damage modifier text
    public void UpdateDamageText(float damageMod)
    {
        damageMultText.text = damageMod.ToString("0.0");
    }
    
    //Set the knockback modifier text
    public void UpdateKnockbackText(float knockbackMod)
    {
        knockbackMultText.text = knockbackMod.ToString("0.0");
    }
    
    //Set the lifesteal modifier text
    public void UpdateLifestealText(float lifestealMod)
    {
        lifestealMultText.text = lifestealMod.ToString("0.0");
    }
    
    //Set the lifesteal modifier text
    public void UpdateCooldownText(float cooldownMod)
    {
        cooldownMultText.text = cooldownMod.ToString("0.0");
    }
    
    //Set the speed modifier text
    IEnumerator FillStatsBar()
    {
        while(statsPanelFillPercent < statsPanelFillTime)
        {
            statsPanelFillPercent += Time.deltaTime; 
            statsPanel.fillAmount = statsPanelFillPercent / statsPanelFillTime;
            yield return null;
        }
    }
    
    //Set the damage modifier text
    IEnumerator UnfillStatsBar()
    {
        while (statsPanelFillPercent > 0)
        {
            statsPanelFillPercent -= Time.deltaTime;
            statsPanel.fillAmount = statsPanelFillPercent/statsPanelFillTime;
            yield return null;
        }

        statsPanelFillPercent = 0;
        statsPanel.fillAmount = 0;
    }
}