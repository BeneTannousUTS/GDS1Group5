using System.Collections;
using UnityEngine;

public class DecoyWobble : MonoBehaviour
{
    public void StartShakeDecoy(Vector3 pushSource)
    {
        Vector3 pushDirection = pushSource - transform.position;
        Vector3 direction = pushDirection.normalized;
        Vector3 originalPosition = transform.position;
        StartCoroutine(ShakeDecoySequence(originalPosition, direction));
        AudioManager.instance.PlaySoundEffect("PlayerDamage", Random.Range(1.3f,1.5f));
    }

    IEnumerator ShakeDecoySequence(Vector3 originalPosition, Vector3 direction)
    {
        float shakeDuration = 0.3f;
        float shakeStrength = 0.3f; 
        float shakeSpeed = 50f;     
    
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Offset the position using a sine wave for smooth motion
            float offset = Mathf.Sin(elapsed * shakeSpeed) * shakeStrength;
            transform.position = originalPosition + direction * offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original position
        transform.position = originalPosition;
    }
}
