// AUTHOR: BENEDICT
// This is a helper script so each component can be manually assigned from the inspector,
// making HUD setup and component management easier and more efficient

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIComponentHelper : MonoBehaviour
{
    public TextMeshProUGUI playerNumTMP;
    public Image primaryAbility;
    public Image secondaryAbility;
    public Image primaryAbilityBackground;
    public Image secondaryAbilityBackground;
    public Image healthSlider;
    public GameObject healthBarPanel; 
    public TextMeshProUGUI healthTextOver;
    public TextMeshProUGUI healthTextUnder;
    public StatsDisplayHelper statsHelper;
    public TextMeshProUGUI scoreText;
    public GameObject scorePanel; 
}
