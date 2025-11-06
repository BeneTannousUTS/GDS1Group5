using UnityEngine;
using UnityEngine.UI;

public class RotateRays : MonoBehaviour
{
    private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.gameObject.transform.Rotate(new Vector3(0f, 0f, 120f * Time.deltaTime));
    }
}
