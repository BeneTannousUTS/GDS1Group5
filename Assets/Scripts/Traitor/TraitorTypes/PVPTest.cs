using UnityEngine;

public class PVPTest : BaseTraitor
{

    public override void TraitorAbility()
    {
        base.TraitorAbility();
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
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 10;
        traitorSprite = traitorManager.GetCardRef(0);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
