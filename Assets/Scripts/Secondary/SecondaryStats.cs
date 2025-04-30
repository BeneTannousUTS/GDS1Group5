// AUTHOR: Alistair
// Handles the storing of secondary stats and destroys the secondary
// after its lifetime is over

using UnityEngine;
using System.Collections;

public class SecondaryStats : MonoBehaviour
{
    public float secondaryLifetime;
    public bool activateOnDestroy = false;
    private string sourceType;
    [SerializeField] private GameObject sourceObject;
    public GameObject projectile;

    // Sets the value of sourceType
    public void SetSourceType(string type)
    {
        sourceType = type;
    }

    public void SetSourceObject(GameObject source) {
        sourceObject = source;
    }

    public GameObject GetSourceObject() {
        return sourceObject;
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

    // Calculates a quaternion which is the rotation needed for the weapon based on direction
    public Quaternion CalculateQuaternion(Vector3 direction) 
    {
        float angle = Mathf.Abs((Mathf.Acos(direction.x) * 180)/Mathf.PI);

        if (direction.y >= 0)
        {
            angle -= 90;
        }
        else if (direction.y < 0)
        {
            angle = 270 - angle;
        }

        return Quaternion.Euler(0f, 0f, angle);
    }

    // Destroys the weapon after its lifetime is up
    IEnumerator DestroySecondary(float lifetime)
    {   
        if (activateOnDestroy == false) 
        {
            gameObject.GetComponent<ISecondary>().DoSecondary();
        }

        yield return new WaitForSeconds(lifetime);

        if (activateOnDestroy == true) 
        {
            gameObject.GetComponent<ISecondary>().DoSecondary();
        }

        Destroy(gameObject);
    }

    // Starts the DestroyWeapon coroutine
    void Start()
    {   
        StartCoroutine(DestroySecondary(secondaryLifetime));
    }
}
