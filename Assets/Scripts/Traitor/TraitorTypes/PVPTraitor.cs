using UnityEngine;

public class PVPTraitor : BaseTraitor
{

    GameObject passive;

    public override void TraitorAbility()
    {
        gameObject.GetComponent<PlayerStats>().SetPassive(passive);
        gameObject.GetComponent<PlayerStats>().public_RemoveTempBuff(6, passive);
    }

    public override void TraitorSetup()
    {
        base.TraitorSetup();
        gameObject.GetComponent<PlayerCollision>().SetPlayerPVP(true);
    }

    public override void LoseCondition()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Traitor");
        if (temp.Length == 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("EscapeDoor"));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 10;
        traitorSprite = traitorManager.GetCardRef(0);
        passive = traitorManager.GetObjectRef(1);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
