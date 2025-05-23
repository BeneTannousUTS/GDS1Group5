using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PolymorthTraitor : BaseTraitor
{
    List<GameObject> playerList = new List<GameObject>();
    public GameObject polyProjectile;
    public override void TraitorAbility()
    {
        base.TraitorAbility();
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject item = Instantiate(polyProjectile, transform.position, CalculateQuaternion(attackDirection), transform);
        item.GetComponent<WeaponStats>().SetSourceType("Traitor");
        item.GetComponent<WeaponStats>().SetSourceObject(gameObject);
    }

    public Quaternion CalculateQuaternion(Vector3 direction)
    {
        float angle = Mathf.Abs((Mathf.Acos(direction.x) * 180) / Mathf.PI);

        if (direction.y >= 0)
        {
            angle -= 90;
        }
        else if (direction.y < 0)
        {
            angle = 270 - angle;
        }

        return Quaternion.Euler(0f, 0f, angle);
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
        cooldownLength = 6;
        traitorSprite = traitorManager.GetCardRef(0);
        polyProjectile = traitorManager.GetObjectRef(7);
        playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(10);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
