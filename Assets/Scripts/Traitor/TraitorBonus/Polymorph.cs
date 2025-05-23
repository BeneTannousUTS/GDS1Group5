using System.Collections;
using UnityEngine;

public class Polymorph : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController morphAnimator;
    [SerializeField] GameObject morphWeapon;
    [SerializeField] GameObject morphSecondary;
    GameObject weapon;
    GameObject secondary;
    RuntimeAnimatorController animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator Morph()
    {
        weapon = gameObject.GetComponent<PlayerAttack>().currentWeapon;
        secondary = gameObject.GetComponent<PlayerSecondary>().currentSecondary;
        animator = gameObject.GetComponent<Animator>().runtimeAnimatorController;
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

    public void RemoveMorph()
    {
        gameObject.GetComponent<PlayerAttack>().currentWeapon = weapon;
        gameObject.GetComponent<PlayerSecondary>().currentSecondary = secondary;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = animator;
        Destroy(gameObject.GetComponent<Polymorph>());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
