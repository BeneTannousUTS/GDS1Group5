using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BomberTraitor : BaseTraitor
{
    List<GameObject> playerList = new List<GameObject>();
    public GameObject passive;
    private GameObject bomb;
    public override void TraitorAbility()
    {
        base.TraitorAbility();
        gameObject.GetComponent<PlayerStats>().SetPassive(passive);
        gameObject.GetComponent<PlayerStats>().public_RemoveTempBuff(5, passive);
        StartCoroutine(DropBombs());
    }

    IEnumerator DropBombs()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject tempSecondary = Instantiate(bomb, transform.position, Quaternion.identity);
            tempSecondary.GetComponent<SecondaryStats>().SetSourceType(gameObject.tag);
            tempSecondary.GetComponent<SecondaryStats>().SetSourceObject(gameObject);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void TraitorSetup()
    {
        base.TraitorSetup();
    }


    public override void LoseCondition()
    {
        DestroyDoor();
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 8;
        traitorSprite = traitorManager.GetCardRef(0);
        passive = traitorManager.GetObjectRef(5);
        bomb = traitorManager.GetObjectRef(6);
        playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(9);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
