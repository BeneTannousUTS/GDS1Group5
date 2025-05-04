using UnityEngine;

public class Barrel : Destructible
{
    public override void SpawnItems()
    {
        GameObject item = Instantiate(spawnedItem);
        item.transform.position = gameObject.transform.position;
        item.GetComponent<Projectile>().SetShotDirection(hitDirection);
        item.GetComponent<Animator>().SetTrigger("vertical");
        item.GetComponent<Projectile>().SetDamageValue(0);
        item.GetComponent<Projectile>().SetFriendlyFire(true);
        item.GetComponent<Projectile>().SetSourceType("Player");
        item.GetComponent<Projectile>().SetSourceObject(gameObject);
        Destroy(gameObject);
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
