using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector3 facingDirection = Vector3.right;

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection() 
    {
        return facingDirection;
    }
}
