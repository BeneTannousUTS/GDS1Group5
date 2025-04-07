//AUTHOR: Benedict
/*This script passes values to the text mesh pro elements in the HUD to show how stats have been influenced
by passive cards*/

using TMPro;
using UnityEngine;

public class StatsDisplayHelper : MonoBehaviour
{
    public TextMeshProUGUI healthMultText;
    public TextMeshProUGUI damageMultText;
    public TextMeshProUGUI speedMultText;

    private float baseHealth;
    private float baseDamage;
    private float baseSpeed;

    private float currentHealth;
    private float currentDamage;
    private float currentSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set the health modifier text
    public void UpdateHealthText(float healthMod)
    {
        healthMultText.text = healthMod + "% Health";
    }
    
    //Set the speed modifier text
    public void UpdateSpeedText(float speedMod)
    {
        speedMultText.text = speedMod * 100 + "% Speed";
    }
    
    //Set the damage modifier text
    public void UpdateDamageText(float damageMod)
    {
        damageMultText.text = damageMod * 100 + "% Damage";
    }
}
