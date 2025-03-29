using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    [SerializeField]
    private int playerNum;

    private GameObject hud;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hud = Instantiate(playerHUDPrefab, GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform, false);
        // hud.GetComponentInChildren<UIComponentHelper>().primaryAbility; 
        // hud.GetComponentInChildren<UIComponentHelper>().secondaryAbility; 
    }

    public void SetPlayerNum(int playerIndex)
    {
        EnsureHUD(); // Make sure the HUD exists before setting values
        playerNum = playerIndex + 1;
        hud.GetComponentInChildren<UIComponentHelper>().playerNumTMP.text = "P" + playerNum;
    }
    
    public void SetHUDColour(Color healthColour)
    {
        EnsureHUD(); // Make sure the HUD exists before setting values
        UIComponentHelper helper = hud.GetComponentInChildren<UIComponentHelper>();
        if (helper != null)
        {
            helper.playerNumTMP.color = healthColour;
            helper.healthSlider.color = healthColour;
        }
    }
    
    

    void EnsureHUD()
    {
        if (hud == null)
        {
            hud = Instantiate(playerHUDPrefab, GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform, false);
        }
    }
    
    
}
