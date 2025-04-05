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

    public void SetBaseStats(float health, float speed, float attack)
    {
        baseHealth = health;
        baseSpeed = speed;
        baseDamage = attack;
    }

    public void UpdateHealthText(float healthMod)
    {
        healthMultText.text = healthMod + "% Health";
    }
    
    public void UpdateSpeedText(float speedMod)
    {
        speedMultText.text = speedMod * 100 + "% Speed";
    }
    
    public void UpdateDamageText(float damageMod)
    {
        damageMultText.text = damageMod * 100 + "% Damage";
    }
}
