using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathBarShakeHandler : MonoBehaviour
{
    
    protected RectTransform rt;
    
    protected virtual void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    
    public void ShakeCard()
    {
        StartCoroutine(Shake());
    }
    
    private IEnumerator Shake()
    {
        Vector3 originalPos = rt.anchoredPosition;

        float shakeDuration = 0.2f;
        float shakeStrength = 20f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeStrength;
            float offsetY = Random.Range(-1f, 1f) * shakeStrength;

            rt.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = originalPos;
    }
}
