//AUTHOR: BENEDICT
//This script handles the small health bar that appears underneath players and enemies

using UnityEngine;
using UnityEngine.UI;

public class SmallHealthBar : MonoBehaviour
{
    Color lowHealthColour;
    Color highHealthColour;
    
    Gradient gradient;
    
    public Image healthBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gradient = new Gradient();
        GradientColorKey[] colours =  new GradientColorKey[2];
        colours[0] = new GradientColorKey(Color.red, 0f);
        colours[1] = new GradientColorKey(Color.green, 1f);
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
}
