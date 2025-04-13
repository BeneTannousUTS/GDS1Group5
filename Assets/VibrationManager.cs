using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    private Coroutine infoCoroutine;
    private bool isInfoVibrating = false;
    
    public enum VibrationPattern
    {
        DamagePattern,
        HealPattern,
        InfoPattern
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DamagePattern(Gamepad controller)
    {
        float vibTime = 0f;
        float vibFreq = 0.75f;
        while (vibTime < 0.2f)
        {
            controller.SetMotorSpeeds(vibFreq,vibFreq) ;
            vibTime += Time.deltaTime;
            vibFreq = -2.5f * vibTime + 0.75f;
            yield return null;
        }
        
        controller.SetMotorSpeeds(0,0);
    }
    
    IEnumerator HealPattern(Gamepad controller)
    {
        float vibTime = 0f;
        float vibFreq = 0f;
        while (vibTime < 0.3f)
        {
            controller.SetMotorSpeeds(vibFreq,vibFreq) ;
            vibTime += Time.deltaTime;
            vibFreq = 2.5f * vibTime;
            yield return null;
        }
        
        controller.SetMotorSpeeds(0,0);
    }

    public void StartVibrationPattern(Gamepad controller, VibrationPattern vibrationPattern)
    {
        switch (vibrationPattern)
        {
            case VibrationPattern.DamagePattern:
                StartCoroutine(DamagePattern(controller));
                break;
            case VibrationPattern.HealPattern:
                StartCoroutine(HealPattern(controller));
                break;
            default:
                return;
        }
    }
    
    /*------------------------------------ Coroutine Logic -----------------------------*/

    IEnumerator InfoPattern(Gamepad controller)
    {
        isInfoVibrating = true;

        while (isInfoVibrating)
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
        isInfoVibrating = false;
    }
    
    public void StartInfoVibration(Gamepad controller)
    {
        if (!isInfoVibrating)
        {
            infoCoroutine = StartCoroutine(InfoPattern(controller));
        }
    }
   
    public void StopInfoVibration()
    {
        isInfoVibrating = false;
    }
}
