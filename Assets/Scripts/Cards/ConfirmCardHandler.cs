using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class ConfirmCardHandler : MonoBehaviour
{
    public bool hasConfirmed = false;
    public bool? confirmedChoice = null;
    public TMP_Text playerText;
    public Image playerIcon;
    public Image prevCard;
    public Image newCard;
    public TMP_Text yesText;
    public TMP_Text noText;
    public int playerIndex;
    public PlayerInput assignedInput = null;

    public void ShakeCard()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector3 originalPos = rt.anchoredPosition;

        float shakeDuration = 0.3f;
        float shakeStrength = 2f;
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
