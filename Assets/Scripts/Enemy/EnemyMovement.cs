// AUTHOR: Alistair
// Handles enemy movement and stores ai type

using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public enum aiType
    {
        Aggressive,
        Passive,
        Teleporter,
        Cautious,
        Stationary,
        Charger
    }

    public aiType currentAiType;

    public float attackRange;
    public float aggroRange;
    public float moveSpeed;
    public float chargeMultiplier;

    private Vector3 movementOffset = Vector3.zero;

    private EnemyPathfinder enemyPathfinder;

    private GameObject movePoint;

    private Vector3 facingDirection = Vector3.right;

    private bool teleporting = false;
    public GameObject teleportParticles;

    private Animator animator;
    private SpriteRenderer sprite;
    private float knockbackTime = 0.0f;
    private float dashTime = 0f;
    private float frozenTimer = 3f;

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

                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position + movementOffset, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        }
    }

    // Charger AI movement meant to run towards closest player then charge when attacking
    void MoveCharger()
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

                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position + movementOffset, moveSpeed * Time.deltaTime);
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

    // Cautious AI movement meant move in range of the player but stay at range
    void MoveCautious()
    {
        if (movePoint != null && movePoint.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

            if (Vector3.Distance(movePoint.transform.position, transform.position) <= attackRange) 
            {
                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position + movementOffset, -1 * moveSpeed * Time.deltaTime);
            }
            else if (Vector3.Distance(movePoint.transform.position, transform.position) <= aggroRange)
            {
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(true);
            }
            else
            {
                movePoint = enemyPathfinder.ClosestPlayer(transform.position);

                gameObject.GetComponent<EnemyAttack>().SetCanAttack(false);

                transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position + movementOffset, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        }
    }

    // Teleporter AI movement meant to teleport to the random position if the players are too close
    void MoveTeleporter()
    {
        if (movePoint != null && movePoint.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);

            facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

            if (Vector3.Distance(movePoint.transform.position, transform.position) <= aggroRange) 
            {
                if (teleporting == false) {
                    StartCoroutine(Teleport());
                }
                gameObject.GetComponent<EnemyAttack>().SetCanAttack(false);
            }
            else if (Vector3.Distance(movePoint.transform.position, transform.position) <= attackRange) 
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

    // Stationary AI meant to stay still and fire at players when in range
    void MoveStationary()
    {
        if (movePoint != null && movePoint.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);

            facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

            if (Vector3.Distance(movePoint.transform.position, transform.position) <= attackRange) 
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

    IEnumerator Teleport() 
    {
        teleporting = true;

        Instantiate(teleportParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.3f);
        animator.SetTrigger("teleport");
        yield return new WaitForSeconds(0.2f);
        Vector3 summonPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
        AudioManager.instance.PlaySoundEffect("Teleport");
        bool validPos = false;
        float exitTime = 0;
        while (!validPos)
        {
            exitTime++;
            Vector3 checkPos = new Vector3(Random.Range(-14f, 14f), Random.Range(-6, 5), 0) + summonPos;
            Collider2D[] hit = Physics2D.OverlapCircleAll(checkPos, 1.5f, LayerMask.GetMask("Default"));
            if (hit.Length == 0 || exitTime == 100)
            {
                validPos = true;
                transform.position = checkPos;
            }
        }

        teleporting = false;
    }

    IEnumerator ChangeOffset() 
    {
        yield return new WaitForSeconds(0.5f);

        movementOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.8f, 0.8f), 0f);

        StartCoroutine(ChangeOffset());
    }

    public void ActivateAttackSpecial() {
        if (currentAiType == aiType.Charger) 
        {
            GetComponent<Rigidbody2D>().linearVelocity = facingDirection.normalized * moveSpeed * chargeMultiplier;
            dashTime = Random.Range(0.2f, 0.4f);
        }
    }

    // Get a reference to the enemyPathfinder
    void Start()
    {
        enemyPathfinder = GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>();
        movePoint = enemyPathfinder.ClosestPlayer(transform.position);
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(ChangeOffset());
    }

    // Move based on the AI type
    void Update()
    {
        frozenTimer += Time.deltaTime;
        if (frozenTimer >= 3f) {
            if (knockbackTime > 0)
            {
                knockbackTime -= Time.deltaTime;
            }
            else if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
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
                else if (currentAiType == aiType.Cautious)
                {
                    MoveCautious();
                }
                else if (currentAiType == aiType.Stationary)
                {
                    MoveStationary();
                }
                else if (currentAiType == aiType.Charger)
                {
                    MoveCharger();
                }

                SetSpriteDirection();
                if (facingDirection.x == 0 && facingDirection.y == 0) animator.SetBool("isMoving", false);
                else animator.SetBool("isMoving", true);
            }
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
        if (currentAiType != aiType.Stationary) {
            GetComponent<Rigidbody2D>().linearVelocity = knockbackDirection.normalized * moveSpeed * knockbackMultiplier;
            this.knockbackTime = knockbackTime;
        }
    }

    public void ResetMovePoint() 
    {
        movePoint = enemyPathfinder.ClosestPlayer(transform.position);
    }

    public void SetFrozen() 
    {
        frozenTimer = 0f;
        gameObject.GetComponent<EnemyAttack>().SetCanAttack(false);
    }
}
