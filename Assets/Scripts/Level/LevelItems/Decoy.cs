// AUTHOR: ALISTAIR

using UnityEngine;

public class Decoy : Destructible
{
    public float lifetimeWindow = 7f;
    public float lifetimeTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach(GameObject enemy in enemies) 
        {
            enemy.GetComponent<EnemyMovement>().ResetMovePoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetimeWindow) 
        {
            Destroy(gameObject);
        }
    }
}
