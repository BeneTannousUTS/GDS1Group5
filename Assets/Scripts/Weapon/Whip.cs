using UnityEngine;

public class Whip : WeaponStats
{
   protected override float GetDamageValue(HealthComponent healthComponent) 
   {
        float dist = Vector3.Distance(sourceObject.transform.position, healthComponent.gameObject.transform.position);
        
        float mod = dist * 1.5f;
        if (mod <= 1f) {
            mod = 1f;
        }

        if (mod >= 3f) {
            mod = 3f;
        }

        return damageValue * mod;
   }
}
