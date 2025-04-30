// AUTHOR: ALISTAIR

using UnityEngine;

public class Barrier : Destructible
{
    public float lifetimeWindow = 5f;
    public float lifetimeTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
