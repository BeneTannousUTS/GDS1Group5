using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisablerTraitor : BaseTraitor
{
    List<GameObject> playerList = new List<GameObject>();
    public GameObject weapon;
    public Sprite disableIcon;
    public override void TraitorAbility()
    {
        foreach (GameObject player in playerList)
        {
            base.TraitorAbility();
            if (player != gameObject)
            {
                player.GetComponent<PlayerAttack>().Attack(weapon);
                player.GetComponent<PlayerIcons>().SetIcon(disableIcon, 5);
            }
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
        cooldownLength = 14;
        traitorSprite = traitorManager.GetCardRef(0);
        weapon = traitorManager.GetObjectRef(3);
        playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(6);
        disableIcon = traitorManager.GetIcon(0);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
