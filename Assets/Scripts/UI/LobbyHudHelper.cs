using UnityEngine;

public class LobbyHudHelper : MonoBehaviour
{
    public GameObject[] joinPanels;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateJoinPanel(int index)
    {
        joinPanels[index].SetActive(false);
    }
    
    public void ReactivateJoinPanel(int index)
    {
        joinPanels[index].SetActive(true);
    }
}
