using System.Collections.Generic;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject spawnedItem;
    public bool playerCanHurt = true;
    protected Vector3 hitDirection;

    private HashSet<int> processedColliderIDs = new HashSet<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int colliderID = collision.GetInstanceID();
        if (processedColliderIDs.Contains(colliderID)) return;
        if (!collision.CompareTag("Hazard"))
        {
            processedColliderIDs.Add(colliderID);
        }
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (collision.gameObject.CompareTag("Weapon") && (playerCanHurt || !collision.gameObject.GetComponent<WeaponStats>().GetSourceType().Equals("Player")))
            {
                collision.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
                hitDirection = (collision.transform.position - collision.transform.parent.transform.position).normalized;
            }
            else if (collision.gameObject.CompareTag("Projectile") && (playerCanHurt || !collision.gameObject.GetComponent<Projectile>().GetSourceType().Equals("Player")))
            {
                collision.GetComponent<Projectile>().DealDamage(gameObject.GetComponent<HealthComponent>());
                hitDirection = collision.GetComponent<Projectile>().GetShotDirection();
            }
        }
    }

    public virtual void SpawnItems()
    {
        if (Random.Range(0, 2) == 1)
        {
            Instantiate(spawnedItem).transform.position = gameObject.transform.position;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
