using UnityEngine;

public class DoorPressurePlate : MonoBehaviour
{
    [SerializeField] GameObject[] plates;
    
    public int CountActivePlates()
    {
        int count = 0;
        foreach (GameObject plate in plates)
        {
            if (plate.TryGetComponent<PressurePlate>(out var pressurePlate))
            {
                if (pressurePlate.GetSwitchActivated())
                    count++;
            }
        }
        return count;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ActivePlates(FindAnyObjectByType<GameManager>().GetPlayerList().Count);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CheckShouldDoorOpen(int activePlayers)
    {
    }
}
