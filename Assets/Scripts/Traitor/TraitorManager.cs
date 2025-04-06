// AUTHOR: James
// Manages when a traitor card should appear

using UnityEngine;

public class TraitorManager : MonoBehaviour
{
    private ITraitor traitorType;
    private float traitorRoom;
    private bool traitorCardAppear;
    private DungeonManager dungeonManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Called by game manager when the traitor is decided
    public void SetTraitorType(ITraitor traitor)
    {
        traitorType = traitor;
        traitorRoom = dungeonManager.GetDungeonLength() - traitorType.getTraitorRoom();
    }

    //called by the card manager when cards are being decided to tell if the traitor card should appear
    public bool CheckTraitorAppear()
    {
        if (dungeonManager.GetRoomCount() == traitorRoom)
        {
            return true;
        }
        return false;
    }
    void Start()
    {
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
