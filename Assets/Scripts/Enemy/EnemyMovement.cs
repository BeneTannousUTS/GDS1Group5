// AUTHOR: Alistair
// Handles enemy movement and stores ai type

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum aiType {
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

    // Gets the value of facingDirection
    public Vector3 GetFacingDirection() 
    {
        return facingDirection;
    }

    // Aggressive AI movement meant to run towards closest player
    void MoveAggressive() 
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

    // Passive AI movement meant to run to be a certain distance from the closest player
    void MovePassive() 
    {
        facingDirection = new Vector3(movePoint.transform.position.x - transform.position.x, movePoint.transform.position.y - transform.position.y, 0f);

        if (Vector3.Distance(movePoint.transform.position, transform.position) < aggroRange)
        {
            movePoint = enemyPathfinder.ClosestPlayer(transform.position);

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

    // STRETCH GOAL: Teleporter AI movement meant to teleport to the furthest position from the closest player
    void MoveTeleporter() 
    {

    }

    // Get a reference to the enemyPathfinder
    void Start()
    {
        enemyPathfinder = GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>();
        movePoint = enemyPathfinder.ClosestPlayer(transform.position);
    }

    // Move based on the AI type
    void Update() {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
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
        }
    }
}
