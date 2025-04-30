using System.Collections;
using UnityEngine;

public class Polymorph : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController morphAnimator;
    [SerializeField] GameObject morphWeapon;
    [SerializeField] GameObject morphSecondary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator Morph()
    {
        GameObject weapon = gameObject.GetComponent<PlayerAttack>().currentWeapon;
        GameObject secondary = gameObject.GetComponent<PlayerSecondary>().currentSecondary;
        RuntimeAnimatorController animator = gameObject.GetComponent<Animator>().runtimeAnimatorController;
        gameObject.GetComponent<PlayerAttack>().currentWeapon = morphWeapon;
        gameObject.GetComponent<PlayerSecondary>().currentSecondary = morphSecondary;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = morphAnimator;
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<PlayerAttack>().currentWeapon = weapon;
        gameObject.GetComponent<PlayerSecondary>().currentSecondary = secondary;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = animator;
        Destroy(gameObject.GetComponent<Polymorph>());
    }

    public void SetupMorph(RuntimeAnimatorController anim, GameObject wep, GameObject sec)
    {
        morphAnimator = anim;
        morphWeapon = wep;
        morphSecondary = sec;
        StartCoroutine(Morph());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
