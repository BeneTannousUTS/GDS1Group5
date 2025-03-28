// AUTHOR: Alistair
// Handles the storing of secondary stats and destroys the secondary
// after its lifetime is over

using UnityEngine;
using System.Collections;

public class SecondaryStats : MonoBehaviour
{
    public float secondaryLifetime;
    private string sourceType;
    public GameObject projectile;

    // Sets the value of sourceType
    public void SetSourceType(string type)
    {
        sourceType = type;
    }

    // Gets the value of sourceType
    public string GetSourceType() 
    {
        return sourceType;
    }

    // Gets the value of projectile
    public GameObject GetProjectile() 
    {
        return projectile;
    }

    // Destroys the weapon after its lifetime is up
    IEnumerator DestroySecondary(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.GetComponent<ISecondary>().DoSecondary();
        Destroy(gameObject);
    }

    // Starts the DestroyWeapon coroutine
    void Start()
    {   
        StartCoroutine(DestroySecondary(secondaryLifetime));
    }
}
