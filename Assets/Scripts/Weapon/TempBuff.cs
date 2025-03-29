using UnityEngine;
using System.Collections;

public class TempBuff : MonoBehaviour
{
    public GameObject passive;
    public float buffLifetime;
    public float buffTime;

    public GameObject GetPassive() 
    {
        return passive;
    }

    public float GetBuffTime() 
    {
        return buffTime;
    }

    IEnumerator DestroyTempBuff(float lifetime) 
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(DestroyTempBuff(buffLifetime));

        transform.position += 0.001f * transform.up;
    }
}
