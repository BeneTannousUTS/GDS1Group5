using System;
using System.Collections;
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
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                collision.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
                hitDirection = (collision.transform.position - collision.transform.parent.transform.position).normalized;
            }
            else if (collision.gameObject.CompareTag("Projectile"))
            {
                collision.GetComponent<Projectile>().DealDamage(gameObject.GetComponent<HealthComponent>());
                hitDirection = collision.GetComponent<Projectile>().GetShotDirection();
                if (collision.GetComponent<Explosion>() != null)
                {
                    collision.GetComponent<Explosion>().SpawnExplosion();
                }
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator RemoveDestructible()
    {
        yield return new WaitForSeconds(0.4f);
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            Instantiate(spawnedItem).transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    }

    public virtual void SpawnItems()
    {
        gameObject.GetComponent<Animator>().SetTrigger("break");
        StartCoroutine(RemoveDestructible());
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
