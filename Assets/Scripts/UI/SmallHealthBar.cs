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
        gradient.colorKeys = new GradientColorKey[2];
        gradient.colorKeys[0] = new GradientColorKey(Color.red, 0f);
        gradient.colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
        gradient.colorKeys[2] = new GradientColorKey(Color.green, 1f);
    }

    public void SetHealthBarFill(float healthBarFill)
    {
        healthBar.fillAmount = healthBarFill;
    }

    void EvaluateColour()
    {
        healthBar.color = gradient.Evaluate(healthBar.fillAmount);
    }
}
