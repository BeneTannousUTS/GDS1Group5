//AUTHOR: BENEDICT
//This script handles the small health bar that appears underneath players and enemies

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmallHealthBar : MonoBehaviour
{
    Color lowHealthColour;
    Color highHealthColour;
    
    Gradient gradient;
    
    public Image healthBar;
    public TextMeshProUGUI healthText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gradient = new Gradient();
        GradientColorKey[] colours =  new GradientColorKey[3];
        colours[0] = new GradientColorKey(Color.red, 0f);
        colours[1] = new GradientColorKey(Color.yellow, 0.5f);
        colours[2] = new GradientColorKey(Color.green, 1f);
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(255, 0);
        alphas[1] = new GradientAlphaKey(255, 1);
        gradient.SetKeys(colours, alphas);
    }

    public void SetHealthBarFill(float healthBarFill)
    {
        if (healthBarFill == 1)
        {
            healthBar.enabled = false;
        }else if(healthBarFill < 1 && healthBarFill > 0)
        {
            healthBar.enabled = true;
            healthBar.fillAmount = healthBarFill;
            EvaluateColour();
        }else if(healthBarFill == 0)
        {
            healthBar.enabled = false;
        }
    }

    void EvaluateColour()
    {
        healthBar.color = gradient.Evaluate(healthBar.fillAmount);
    }

    public void SetHealthBarText(float currentHealth, float maxHealth)
    {
        if (healthText == null)
        {
            return;
        }

        healthText.text = currentHealth.ToString("0") + "/" + maxHealth.ToString("0");
        if (currentHealth == maxHealth)
        {
            healthText.alpha = 0;
        }
        else
        {
            healthText.alpha = 255;
        }
    }
}
