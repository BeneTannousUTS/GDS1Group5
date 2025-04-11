// AUTHOR: James
// Handles the enemy debug menu

using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class DebugEnemyUI : MonoBehaviour
{
    GameObject enemyObject;
    List<GameObject> enemySpawned = new List<GameObject>();
    [SerializeField] GameObject[] enemies;
    GameManager gameManager;
    [SerializeField] TMP_Text giveBtn;
    public TMP_Dropdown dropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void DeleteEnemies()
    {
        foreach (GameObject enemy in enemySpawned)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemySpawned.Clear();
    }
    public void SpawnEnemy()
    {
        if (enemyObject != null)
        {
            GameObject enemy = Instantiate(enemyObject);
            Vector3 summonPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
            bool validPos = false;
            float exitTime = 0;
            while (!validPos)
            {
                exitTime++;
                Vector3 checkPos = new Vector3(Random.Range(-14f, 14f), Random.Range(-6, 5), 0) + summonPos;
                Collider2D[] hit = Physics2D.OverlapCircleAll(checkPos, 1.5f, LayerMask.GetMask("Default"));
                if (hit.Length == 0 || exitTime == 100)
                {
                    validPos = true;
                    enemy.transform.position = checkPos;
                }
            }
            enemySpawned.Add(enemy);
        }
    }

    public void EnemyDropdown()
    {
        enemyObject = enemies[dropdown.value];
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (GameObject enemy in enemies)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(enemy.name));
        }
        dropdown.value = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
