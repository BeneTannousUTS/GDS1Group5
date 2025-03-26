// AUTHOR: James
// Handles each room functions such as opening door

using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    private bool openDoor;
    private EnemyMovement[] enemyCount;
    private float checkTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Checks if there are still enemies in the room, if not then destroy the door (only checks every second)
    void CheckEnemy()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer > 1f)
        {
            openDoor = true;
            foreach (EnemyMovement enemy in enemyCount)
            {
                if (enemy != null)
                {
                    openDoor = false;
                }
            }
            if (openDoor)
            {
                Destroy(door);
            }
            checkTimer = 0;
        }
    }
    void Start()
    {
        enemyCount = transform.GetComponentsInChildren<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }
}
