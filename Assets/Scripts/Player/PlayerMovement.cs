// AUTHOR: Alistair
// Handles player movement input

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private Vector3 facingDirection = Vector3.up;
    private Vector3 playerVelocity;
    private CharacterController controller;

    private Vector2 movementInput;
    
    public float moveSpeed;

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection()
    {
        return facingDirection;
    }

    // Setting the value of rigidBody
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // Update moveDirection and facingDirection then linearVelocity in moveDirection
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            moveDirection = new Vector3(movementInput.x, movementInput.y, 0f);
            controller.Move(moveDirection * Time.deltaTime * moveSpeed);
            
            // Setting facingDirection to a vector with
            if (moveDirection != Vector3.zero)
            {
                gameObject.transform.up = moveDirection;
                facingDirection = moveDirection;
            }
            
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}
