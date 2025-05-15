// AUTHOR: Alistair
// Handles player movement input

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private Vector3 facingDirection = Vector3.down;
    private Vector3 playerVelocity;
    //private CharacterController controller;
    public Rigidbody2D rb;

    private Vector2 movementInput;

    public float moveSpeed;
    private Animator animator;
    private SpriteRenderer sprite;
    private float knockbackTime = 0.0f;
    private float dashTime = 0.0f;

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection()
    {
        return facingDirection;
    }

    // Setting the value of rigidBody
    void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // Update moveDirection and facingDirection then linearVelocity in moveDirection
    void Update()
    {
        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
        }
        else if (dashTime > 0) {
            dashTime -= Time.deltaTime;
        }
        else
        {
            moveDirection = new Vector3(movementInput.x, movementInput.y, 0f);
            rb.linearVelocity = moveDirection.normalized * moveSpeed * gameObject.GetComponent<PlayerStats>().GetMoveStat();
            GetComponent<PlayerScore>().SetTopSpeed(rb.linearVelocity.magnitude);

            // Setting facingDirection to a vector with
            if (moveDirection != Vector3.zero)
            {
                animator.SetBool("isMoving", true);
                if (gameObject.GetComponent<PlayerAttack>().GetHoldingAttack() == false) {
                    SetSpriteDirection();
                    if (moveDirection.x > 0 && sprite.flipX)
                    {
                        sprite.flipX = false;
                    }
                    else if (moveDirection.x < 0 && !sprite.flipX)
                    {
                        sprite.flipX = true;
                    }
                    //gameObject.transform.up = moveDirection;
                    facingDirection = moveDirection.normalized;
                }
            }
            else animator.SetBool("isMoving", false);
        }

        //controller.Move(moveDirection * Time.deltaTime * moveSpeed * gameObject.GetComponent<PlayerStats>().GetMoveStat());
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    // Sets the direction of the sprite in the animator
    private void SetSpriteDirection()
    {
        animator.SetBool("isFront", false);
        animator.SetBool("isFrontDiag", false);
        animator.SetBool("isSide", false);
        animator.SetBool("isBackDiag", false);
        animator.SetBool("isBack", false);
        if (moveDirection.x <= 0.15 && moveDirection.x >= -0.15 && moveDirection.y <= 0)
        {
            animator.SetBool("isFront", true);
        }
        else if ((moveDirection.x >= 0.15 || moveDirection.x <= -0.15) && moveDirection.y <= -0.15)
        {
            animator.SetBool("isFrontDiag", true);
        }
        else if (moveDirection.y >= -0.15 && moveDirection.y <= 0.15)
        {
            animator.SetBool("isSide", true);
        }
        else if ((moveDirection.x >= 0.15 || moveDirection.x <= -0.15) && moveDirection.y >= 0.15)
        {
            animator.SetBool("isBackDiag", true);
        }
        else if (moveDirection.x <= 0.15 && moveDirection.x >= -0.15 && moveDirection.y >= 0)
        {
            animator.SetBool("isBack", true);
        }
    }

    public void KnockbackPlayer(float knockbackMultiplier, float knockbackTime, Vector3 knockbackDirection)
    {
        moveDirection = knockbackDirection.normalized;
        rb.linearVelocity = moveDirection * moveSpeed * knockbackMultiplier;
        this.knockbackTime = knockbackTime;
    }

    public void StartDash() 
    {
        rb.linearVelocity = facingDirection.normalized * moveSpeed * 4f;
        dashTime = 0.2f;
    }
}
