using UnityEngine;

public class PhaseBlink : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;
    [SerializeField] float maxBlinkDistance = 2f;

    public void DoSecondary()
    {
        Transform playerTransform = GetComponent<SecondaryStats>().GetSourceObject().transform;
        PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();
        Vector3 facingDirection = playerMovement.GetFacingDirection().normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(playerTransform.position, facingDirection, maxBlinkDistance);

        Vector3 targetPosition = playerTransform.position + facingDirection * maxBlinkDistance;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null)
                continue;

            GameObject hitObj = hit.collider.gameObject;

            // Skip player
            if (hitObj.CompareTag("Player"))
                continue;

            // If it's a trigger, only stop if it's the room trigger
            if (hit.collider.isTrigger && !hitObj.GetComponent<DungeonCamera>())
                continue;

            // Valid stop
            targetPosition = (Vector3)hit.point - facingDirection * 0.2f;
            break;
        }

        playerTransform.position = targetPosition;
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
