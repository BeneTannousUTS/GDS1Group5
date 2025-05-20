using System.Collections;
using UnityEngine;

public class Ticking : MonoBehaviour
{
        void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        AudioManager.instance.PlaySoundEffect("Ticking");
        yield return new WaitForSeconds(1.75f);
        StartCoroutine(Tick());
    }
}
