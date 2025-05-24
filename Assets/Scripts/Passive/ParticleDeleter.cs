using UnityEngine;
using System.Collections;

public class ParticleDeleter : MonoBehaviour
{
    public float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeleteMe());
    }

    IEnumerator DeleteMe() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
