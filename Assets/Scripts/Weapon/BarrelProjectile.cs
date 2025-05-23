using UnityEngine;

public class BarrelProjectile : MonoBehaviour
{
    public GameObject Explosion;
    public void SpawnExplosion()
    {
        GameObject Explode = Instantiate(Explosion);
        Explode.transform.position = gameObject.transform.position;
        Explode.GetComponentInChildren<WeaponStats>().SetSourceType(gameObject.tag);
        Explode.GetComponentInChildren<WeaponStats>().SetSourceObject(gameObject);
        Explode.GetComponentInChildren<WeaponStats>().SetDamageMod(1);
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
