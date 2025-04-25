using UnityEngine;

public class DoorPressurePlate : MonoBehaviour
{
    [SerializeField] GameObject[] plates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void ActivePlates(int num)
    {
        for (int i = num; i < plates.Length; i++)
        {
            plates[i].SetActive(false);
        }
    }
    
    void Start()
    {
        ActivePlates(FindAnyObjectByType<GameManager>().GetPlayerList().Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
