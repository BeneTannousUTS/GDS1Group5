using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] Sprite[] coinSprites;
    [SerializeField] GameObject coinSparkle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScore>().AddScore(500);
            Instantiate(coinSparkle, gameObject.transform.position, Quaternion.identity);
            AudioManager.instance.PlaySoundEffect("Coins");
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = coinSprites[Random.Range(0, coinSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
