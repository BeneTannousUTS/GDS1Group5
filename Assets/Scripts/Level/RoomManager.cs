// AUTHOR: James
// Handles each room functions such as opening door

using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    private bool openDoor;
    private bool doorBeingDestroyed = false;
    private EnemyMovement[] enemyCount;
    private float checkTimer;
    private AudioManager audioManager;
    private DungeonManager dungeonManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Checks if there are still enemies in the room, if not then destroy the door (only checks every second)
    void CheckEnemy()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer > 1f)
        {
            if (dungeonManager.GetAutoDefeat())
            {
                ClearRoom();
            }
            openDoor = true;
            foreach (EnemyMovement enemy in enemyCount)
            {
                if (enemy != null)
                {
                    openDoor = false;
                }
            }
            if (dungeonManager.GetAutoUnlock())
            {
                openDoor = true;
            }
            if (openDoor)
            {
                if (door & !doorBeingDestroyed) {
                    audioManager.PlaySoundJingle("RoomClear");
                    door.GetComponent<Animator>().SetTrigger("open");
                    Destroy(door, 1.2f);
                    doorBeingDestroyed = true;
                }
                
            }
            checkTimer = 0;
        }
    }

    public void ClearRoom()
    {
        foreach (EnemyMovement enemy in enemyCount)
        {
            if (enemy != null)
            {
                enemy.GetComponent<HealthComponent>().TakeDamage(100000);
            }
        }
    }
    void Start()
    {
        enemyCount = transform.GetComponentsInChildren<EnemyMovement>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }
}
