using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] GameObject[] pots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(pots[Random.Range(0, pots.Length)]).transform.position = gameObject.transform.position + Vector3.up*0.25f;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
