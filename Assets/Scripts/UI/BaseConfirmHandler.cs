// AUTHOR: Zac
// Parent class of all confirming UI elements (like the card confirm pop up)
// MAKE SURE TO CALL INIT OR IT WILL NOT WORK!!

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class BaseConfirmHandler : MonoBehaviour
{
    public bool hasConfirmed = false;
    public bool? confirmedChoice = null;
    public int playerIndex;
    public PlayerInput assignedInput = null;

    protected RectTransform rt;

    protected virtual void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public virtual void init(PlayerInput input)
    {
        assignedInput = input;
        playerIndex = input.playerIndex;
    }

    public void ShakeCard()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
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
