using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] potAnims;
    [SerializeField] Sprite[] pots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = pots[Random.Range(0, pots.Length)];
        gameObject.GetComponent<Animator>().runtimeAnimatorController = potAnims[Random.Range(0, potAnims.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
