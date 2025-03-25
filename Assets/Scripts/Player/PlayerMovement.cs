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

    // Returns 1 or -1 based on the sign of the input
    float NormalizeFloat(float num) 
    {
        return Mathf.Abs(num)/num;
    }

    // Setting the value of rigidBody
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update moveDirection and facingDirection then linearVelocity in moveDirection
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        
        // Setting facingDirection to a vector with 
        if (moveDirection != Vector3.zero) 
        {
            if (moveDirection.x == 0f) 
            {
                facingDirection = new Vector3(0f, NormalizeFloat(moveDirection.y), 0f);
            }
            else if (moveDirection.y == 0f) 
            {
                facingDirection = new Vector3(NormalizeFloat(moveDirection.x), 0f, 0f);
            }
            else 
            {
                facingDirection = new Vector3(NormalizeFloat(moveDirection.x), NormalizeFloat(moveDirection.y), 0f);
            }
        }
        
        rigidBody.linearVelocity = moveDirection.normalized * moveSpeed;
    }
}
