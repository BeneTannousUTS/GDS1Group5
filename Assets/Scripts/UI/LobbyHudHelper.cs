using UnityEngine;

public class LobbyHudHelper : MonoBehaviour
{
    public GameObject[] joinPanels;
    public GameObject leavePrompt;
    private float timeSinceLastShake = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckShouldActivateLeavePrompt();
        if (joinPanels.Length > 0)
        {
            CheckShouldShakeJoinPrompt(Time.deltaTime);
        }
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

    public void DestroyLeavePrompt()
    {
        Destroy(leavePrompt);
    }

    void CheckShouldActivateLeavePrompt()
    {
        if (joinPanels != null && leavePrompt != null)
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

    void CheckShouldShakeJoinPrompt(float time)
    {
        timeSinceLastShake += time;
        if (timeSinceLastShake > 3.0){
            foreach (var panel in joinPanels)
            {
                foreach (var shakeHandler in panel.GetComponentsInChildren<HUDElementShakeHandler>())
                {
                    shakeHandler.ShakeCard();
                }
                Debug.Log("Shaking Join Prompts");
            }
            timeSinceLastShake = 0;
        }
    }
}
