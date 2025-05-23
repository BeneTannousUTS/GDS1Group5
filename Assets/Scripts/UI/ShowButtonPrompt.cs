using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowButtonPrompt : MonoBehaviour
{
    [SerializeField] GameObject buttonPrompt;
    [SerializeField] CircleCollider2D collider;

    public void DestroyObject()
    {
        Destroy(buttonPrompt);
        Destroy(collider);
        Destroy(this);
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
