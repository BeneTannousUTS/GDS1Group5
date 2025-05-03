using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponTraitor : BaseTraitor
{
    List<GameObject> playerList = new List<GameObject>();
    public GameObject[] weapons;
    public override void TraitorAbility()
    {
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        foreach (GameObject weapon in weapons)
        {
            bool isMelee = weapon.GetComponent<WeaponStats>().projectile == null;
            if (!isMelee) GetComponent<PlayerScore>().IncrementProjectilesShot();
            float weaponTypeMod = isMelee ? 1.5f : 0.7f;
            GameObject tempWeapon = Instantiate(weapon, transform.position + attackDirection * weaponTypeMod, CalculateQuaternion(attackDirection), transform);
            tempWeapon.GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
            tempWeapon.GetComponent<WeaponStats>().SetSourceObject(gameObject);
            tempWeapon.GetComponent<WeaponStats>().SetDamageMod(gameObject.GetComponent<PlayerStats>().GetStrengthStat());
            attackDirection = -attackDirection;
        }
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
        cooldownLength = 2;
        traitorSprite = traitorManager.GetCardRef(0);
        weapons = traitorManager.GetWeapons();
        playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(8);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
