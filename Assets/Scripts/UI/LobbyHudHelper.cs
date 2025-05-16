using UnityEngine;

public class LobbyHudHelper : MonoBehaviour
{
    public GameObject[] joinPanels;
    public GameObject leavePrompt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckShouldActivateLeavePrompt();
    }

    public void DeactivateJoinPanel(int index)
    {
        joinPanels[index].SetActive(false);
    }
    
    public void ReactivateJoinPanel(int index)
    {
        joinPanels[index].SetActive(true);
    }

    public void ActivateLeavePrompt()
    {
        leavePrompt.SetActive(true);
    }

    public void DeactivateLeavePrompt()
    {
        leavePrompt.SetActive(false);
    }

    void CheckShouldActivateLeavePrompt()
    {
        foreach (var panel in joinPanels)
        {
            if (!panel.activeSelf)
            {
                ActivateLeavePrompt();
                return;
            }
            DeactivateLeavePrompt();
        }
    }
}
