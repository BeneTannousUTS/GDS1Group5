using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{

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
            if (collision.gameObject.CompareTag("Weapon"))
            {
                collision.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
            }
            else if (collision.gameObject.CompareTag("Projectile"))
            {
                collision.GetComponent<Projectile>().DealDamage(gameObject.GetComponent<HealthComponent>());
            }
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
