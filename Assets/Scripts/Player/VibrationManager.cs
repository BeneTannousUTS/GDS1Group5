using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VibrationManager : MonoBehaviour
{
    private Coroutine infoCoroutine;
    private bool isHeartbeatVibrating = false;
    private GameObject lastButton;
    
    public enum VibrationPattern
    {
        DamagePattern,
        HealPattern,
        ReminderPattern,
        ExplosionPattern,
        AttackPattern
    }

    private void Update()
    {
        CheckMenuNavVibration();
    }

    void CheckMenuNavVibration()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        if (current != lastButton)
        {
            lastButton = current;
            StartCoroutine(MenuNavPattern(Gamepad.current));
        }
    }

    public void StartVibrationPattern(Gamepad controller, VibrationPattern vibrationPattern)
    {
        if (!(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor))
        {
            switch (vibrationPattern)
            {
                case VibrationPattern.DamagePattern:
                    StartCoroutine(DamagePattern(controller));
                    break;
                case VibrationPattern.HealPattern:
                    StartCoroutine(HealPattern(controller));
                    break;
                case VibrationPattern.ReminderPattern:
                    StartCoroutine(ReminderPattern(controller));
                    break;
                case VibrationPattern.ExplosionPattern:
                    StartCoroutine(ExplosionPattern(controller));
                    break;
                case VibrationPattern.AttackPattern:
                    StartCoroutine(AttackPattern(controller));
                    break;
                default:
                    return;
            }
        }
    }
    
    public void StartHeartbeatVibration(Gamepad controller, float vibrationStrength)
    {
        if (!isHeartbeatVibrating && !(Application.platform == RuntimePlatform.OSXPlayer ||
                                       Application.platform == RuntimePlatform.OSXEditor))
        {
            infoCoroutine = StartCoroutine(HeartbeatPattern(controller, vibrationStrength));
        }
    }

    public void StopHeartbeatVibration()
    {
        isHeartbeatVibrating = false;
    }

    public void StopAllVibrations(Gamepad controller)
    {
        StopAllCoroutines();
        controller.SetMotorSpeeds(0f, 0f);
    }

    /*------------------------------------ Coroutine Logic -----------------------------*/

    IEnumerator MenuNavPattern(Gamepad controller)
    {
        float vibTime = 0f;
        while (vibTime < 0.05f)
        {
            controller.SetMotorSpeeds(0.2f, 0.2f);
            vibTime += Time.deltaTime;
            yield return null;
        }

        controller.SetMotorSpeeds(0, 0);
    }
    
    IEnumerator DamagePattern(Gamepad controller)
    {
        float vibTime = 0f;
        float vibFreq = 0.75f;
        while (vibTime < 0.1f)
        {
            controller.SetMotorSpeeds(vibFreq, vibFreq);
            vibTime += Time.deltaTime;
            vibFreq = -2.5f * vibTime + 0.75f;
            yield return null;
        }

        controller.SetMotorSpeeds(0, 0);
    }

    IEnumerator HealPattern(Gamepad controller)
    {
        float vibTime = 0f;
        float vibFreq = 0f;
        while (vibTime < 0.2f)
        {
            controller.SetMotorSpeeds(vibFreq, vibFreq);
            vibTime += Time.deltaTime;
            vibFreq = 2.5f * vibTime;
            yield return null;
        }

        controller.SetMotorSpeeds(0, 0);
    }


    IEnumerator HeartbeatPattern(Gamepad controller, float vibrationStrength)
    {
        isHeartbeatVibrating = true;

        while (isHeartbeatVibrating)
        {
            // First beat
            controller.SetMotorSpeeds(0.4f, 0.4f);
            yield return new WaitForSeconds(0.08f);

            controller.SetMotorSpeeds(0f, 0f);
            yield return new WaitForSeconds(0.1f);

            // Second beat
            controller.SetMotorSpeeds(0.35f, 0.35f);
            yield return new WaitForSeconds(0.07f);

            controller.SetMotorSpeeds(0f, 0f);
            yield return new WaitForSeconds(0.55f); // Pause between heartbeats
        }

        controller.SetMotorSpeeds(0f, 0f);
        infoCoroutine = null;
        isHeartbeatVibrating = false;
    }

    IEnumerator ReminderPattern(Gamepad controller)
    {
        float vibTime = 0f;
        while (vibTime < 0.3f)
        {
            controller.SetMotorSpeeds(1, 1);
            vibTime += Time.deltaTime;
            yield return null;
        }

        controller.SetMotorSpeeds(0, 0);
    }

    IEnumerator ExplosionPattern(Gamepad controller)
    {
        float vibTime = 0f;
        float vibFreq = 1f;
        while (vibTime < 0.4f)
        {
            controller.SetMotorSpeeds(vibFreq, vibFreq);
            vibTime += Time.deltaTime;
            vibFreq = vibTime + 0.75f;
            yield return null;
        }
        
        controller.SetMotorSpeeds(0, 0);
    }
    
    IEnumerator AttackPattern(Gamepad controller)
    {
        float vibTime = 0f;
        while (vibTime < 0.1f)
        {
            controller.SetMotorSpeeds(0.1f, 0.1f);
            vibTime += Time.deltaTime;
            yield return null;
        }

        controller.SetMotorSpeeds(0, 0);
    }
    
    
}