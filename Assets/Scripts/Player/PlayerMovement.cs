// AUTHOR: Alistair
// Handles player movement input

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private Vector3 facingDirection = Vector3.up;
    private Rigidbody2D rigidBody;
    public float moveSpeed;

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection()
    {
        return facingDirection;
    }

    // Setting the value of rigidBody
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update moveDirection and facingDirection then linearVelocity in moveDirection
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            
            // Setting facingDirection to a vector with
            if (moveDirection != Vector3.zero)
            {
                facingDirection = new Vector3(moveDirection.x, moveDirection.y, 0f);
            }
            
            rigidBody.linearVelocity = moveDirection.normalized * moveSpeed;
        }
        else 
        {
            rigidBody.linearVelocity = Vector3.zero;
        }
    }
}
