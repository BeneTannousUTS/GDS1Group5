// AUTHOR: Alistair
// Handles enemy movement and stores ai type

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum aiType
    {
        Aggressive,
        Passive,
        Teleporter
    }

    public aiType currentAiType;

    public float attackRange;
    public float aggroRange;
    public float moveSpeed;

    private EnemyPathfinder enemyPathfinder;

    private GameObject movePoint;

    private Vector3 facingDirection = Vector3.right;

    private Animator animator;
    private SpriteRenderer sprite;
    private float knockbackTime = 0.0f;

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection()
    {
        return facingDirection;
    }

    // Aggressive AI movement meant to run towards closest player
    void MoveAggressive()
    {
        if (movePoint != null && movePoint.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

            if (Vector3.Distance(movePoint.transform.position, transform.position) > aggroRange)
            {
                movePoint = enemyPathfinder.ClosestPlayer(transform.position);
            }

            if (Vector3.Distance(movePoint.transform.position, transform.position) <= attackRange)
            {
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(true);
            }
            else
            {
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(false);

                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        }
    }

    // Passive AI movement meant to run to be a certain distance from the closest player
    void MovePassive()
    {
        if (movePoint != null && movePoint.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

            if (Vector3.Distance(movePoint.transform.position, transform.position) > aggroRange)
            {
                movePoint = enemyPathfinder.ClosestPlayer(transform.position);
            }
            if (Vector3.Distance(movePoint.transform.position, transform.position) <= aggroRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, -1f * moveSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(movePoint.transform.position, transform.position) >= attackRange)
            {
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(true);
            }
            else
            {
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(false);
            }
        }
        else
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        }
    }

    // STRETCH GOAL: Teleporter AI movement meant to teleport to the furthest position from the closest player
    void MoveTeleporter()
    {

    }

    // Get a reference to the enemyPathfinder
    void Start()
    {
        enemyPathfinder = GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>();
        movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Move based on the AI type
    void Update()
    {
        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
        }
        else if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;

            if (currentAiType == aiType.Aggressive)
            {
                MoveAggressive();
            }
            else if (currentAiType == aiType.Passive)
            {
                MovePassive();
            }
            else if (currentAiType == aiType.Teleporter)
            {
                MoveTeleporter();
            }

            SetSpriteDirection();
            if (facingDirection.x == 0 && facingDirection.y == 0) animator.SetBool("isMoving", false);
            else animator.SetBool("isMoving", true);
        }
    }

    // Sets the direction of the sprite in the animator
    void SetSpriteDirection()
    {
        animator.SetBool("isFront", false);
        animator.SetBool("isSide", false);
        animator.SetBool("isBack", false);
        if (((facingDirection.x <= 0 && facingDirection.x >= facingDirection.y) || (facingDirection.x >= 0 && facingDirection.x < facingDirection.y * -1)) && facingDirection.y < 0)
        {
            animator.SetBool("isFront", true);
        }
        else if (((facingDirection.x >= 0 && facingDirection.x <= facingDirection.y) || (facingDirection.x <= 0 && facingDirection.x > facingDirection.y * -1)) && facingDirection.y > 0)
        {
            animator.SetBool("isBack", true);
        }
        else animator.SetBool("isSide", true);
        if (facingDirection.x > 0 && sprite.flipX) sprite.flipX = false;
        else if (facingDirection.x < 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
    }

    public void KnockbackEnemy(float knockbackMultiplier, float knockbackTime, Vector3 knockbackDirection)
    {
        GetComponent<Rigidbody2D>().linearVelocity = knockbackDirection.normalized * moveSpeed * knockbackMultiplier;
        this.knockbackTime = knockbackTime;
    }
}
