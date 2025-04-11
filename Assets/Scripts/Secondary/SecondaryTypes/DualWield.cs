// AUTHOR: Julian
// This secondary does an extra attack with the players currently equipped weapon

using UnityEngine;

public class DualWield : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public float GetCooldownLength()
    {
        return cooldownLength;
    }

    public void DoSecondary() // current issue: for some reason it can't get the reference of the player object, so it can't get the weapon
    {
        Debug.Log(gameObject.GetComponent<SecondaryStats>().GetSourceObject().transform.name);
        gameObject.transform.parent = gameObject.GetComponent<SecondaryStats>().GetSourceObject().transform;
        GameObject parent = gameObject.transform.parent.gameObject;
        GameObject tempWeapon = parent.GetComponent<PlayerAttack>().currentWeapon;
        Vector3 attackDirection = parent.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject secondWeapon = Instantiate(tempWeapon, transform.position + attackDirection, parent.GetComponent<PlayerAttack>().CalculateQuaternion(attackDirection), transform);
        if (attackDirection.x > 0 && secondWeapon.transform.childCount != 0) { // Flipping the sprite of projectile weapons 
            secondWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (secondWeapon.transform.childCount == 0) { // Flipping the sprite of melee weapons
            secondWeapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        secondWeapon.GetComponent<WeaponStats>().SetSourceType(parent.tag);
        secondWeapon.GetComponent<WeaponStats>().SetSourceObject(parent);
        secondWeapon.GetComponent<WeaponStats>().SetDamageMod(parent.GetComponent<PlayerStats>().GetStrengthStat());
    }
}
