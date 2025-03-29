// AUTHOR: BENEDICT
// This script initialises HUD elements for each player that is joined into a game

using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    private int playerNum;

    private GameObject hud;

    // Set the player number text in the HUD panel
    public void SetPlayerNum(int playerIndex)
    {
        EnsureHUD();
        playerNum = playerIndex + 1;
        hud.GetComponentInChildren<UIComponentHelper>().playerNumTMP.text = "P" + playerNum;
    }
    
    // Set the player colour in the HUD panel
    public void SetHUDColour(Color healthColour)
    {
        EnsureHUD();
        UIComponentHelper helper = hud.GetComponentInChildren<UIComponentHelper>();
        if (helper != null)
        {
            helper.playerNumTMP.color = healthColour;
            helper.healthSlider.color = healthColour;
        }
    }
    
    // Make sure the HUD exists before setting values
    void EnsureHUD()
    {
        if (hud == null)
        {
            hud = Instantiate(playerHUDPrefab, GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform, false);
        }
    }
    
    
}
