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

    public void DoSecondary()
    {
        Transform playerTransform = GetComponent<SecondaryStats>().GetSourceObject().transform;
        GameObject currentWeapon = playerTransform.GetComponent<PlayerAttack>().currentWeapon;
        Debug.Log($"Attack Cooldown Window: {currentWeapon.GetComponent<WeaponStats>().attackCooldownWindow}");
        playerTransform.GetComponent<PlayerSecondary>().SetSecondaryCooldownWindow(currentWeapon.GetComponent<WeaponStats>().attackCooldownWindow * 1.5f);

        bool isMelee = currentWeapon.GetComponent<WeaponStats>().projectile == null;
        if (!isMelee) playerTransform.GetComponent<PlayerScore>().IncrementProjectilesShot();
        float weaponTypeMod = isMelee ? 1.5f : 0.7f;

        Vector3 attackDirection = playerTransform.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject tempWeapon = Instantiate(currentWeapon, transform.position + attackDirection * weaponTypeMod, CalculateQuaternion(attackDirection), playerTransform);
        tempWeapon.AddComponent<DualWield>();

        if (attackDirection.x < 0 && !isMelee && tempWeapon.transform.childCount != 0) {
            tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
        }

        if (attackDirection.y < 0) {
            if (tempWeapon.transform.childCount != 0)
            {
                tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            } else
            {
                tempWeapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            
            if (isMelee)
            {
                tempWeapon.transform.position = tempWeapon.transform.position + attackDirection * 0.3f;
            } else
            {
                tempWeapon.transform.position = tempWeapon.transform.position + attackDirection * 0.3f;
            }
        }

        tempWeapon.GetComponent<WeaponStats>().SetSourceType(playerTransform.tag);
        tempWeapon.GetComponent<WeaponStats>().SetSourceObject(playerTransform.gameObject);
        tempWeapon.GetComponent<WeaponStats>().SetDamageMod(playerTransform.GetComponent<PlayerStats>().GetStrengthStat());

        // this NEEDS to be changed but atm there are no other ways to determine which weapon is being used

        AudioManager.instance.PlaySoundEffect(currentWeapon.GetComponent<WeaponStats>().weaponSound);

        if (currentWeapon.GetComponent<WeaponStats>().projectile == null) // is melee
        {
            playerTransform.GetComponent<Animator>().SetTrigger("attack");
        }
        else
        {
            playerTransform.GetComponent<Animator>().SetTrigger("range");
        }
    }

    public Quaternion CalculateQuaternion(Vector3 direction) 
    {
        float angle = Mathf.Abs((Mathf.Acos(direction.x) * 180)/Mathf.PI);

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
}
