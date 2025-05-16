using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowButtonPrompt : MonoBehaviour
{
    [SerializeField] GameObject buttonPrompt;
    [SerializeField] CircleCollider2D collider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonPrompt.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonPrompt.SetActive(false);
        }
    }
}
