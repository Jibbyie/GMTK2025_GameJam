using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Values")]
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float damageToPlayer = 20f;

    [Header("State Flags")]
    [SerializeField] private bool isRetreating;
    private bool isChasing; 

    [Header("Retreat Logic")]
    [SerializeField] private float retreatTimer;
    [SerializeField] private float retreatDuration = 1f;
    private Vector2 retreatDirection;

    [Header("Wander Logic")]
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderDuration = 3f;
    [SerializeField] private float wanderDirection = 1f;
    [SerializeField] private float wanderSpeed = 3f;

    [Header("Component References")]
    [SerializeField] private EnemyDetectionRadius ERD;
    private Rigidbody2D enemyRB;
    private PlayerMovement player;
    private PlayerLogic playerLogic;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        playerLogic = FindFirstObjectByType<PlayerLogic>();

        // Initialize timers
        retreatTimer = retreatDuration;
        wanderTimer = wanderDuration;
    }

    private void Update()
    {
        if (isRetreating)
        {
            retreatTimer -= Time.deltaTime;
            if (retreatTimer <= 0)
            {
                isRetreating = false;
                retreatTimer = retreatDuration;
            }
        }
        else // Only check for chasing or wandering if not retreating
        {
            // Check the detection radius to see if we should be chasing
            if (ERD != null && ERD.playerDetected)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
                // Handle the wander timer only when wandering
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0)
                {
                    wanderDirection *= -1; // Flip direction
                    wanderTimer = wanderDuration;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRetreating)
        {
            // Calculate direction away from the player once
            if (retreatTimer >= retreatDuration - Time.fixedDeltaTime) // Do this only on the first frame of retreating
            {
                Vector2 directionFromPlayer = (transform.position - player.GetPlayerPosition()).normalized;
                retreatDirection = directionFromPlayer;
            }
            enemyRB.linearVelocity = retreatDirection * enemySpeed;
        }
        else if (isChasing)
        {
            // Chase the player
            Vector2 directionToPlayer = (player.GetPlayerPosition() - transform.position).normalized;
            enemyRB.linearVelocity = new Vector2(directionToPlayer.x * enemySpeed, directionToPlayer.y * enemySpeed);
        }
        else // Wander
        {
            enemyRB.linearVelocity = new Vector2(wanderDirection * wanderSpeed, enemyRB.linearVelocity.y);
        }
    }

    // Handles physical collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRetreating = true;
            playerLogic.TakeDamage(damageToPlayer);
        }
    }

    // Handles trigger collision with the shield
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            // Retreat, but do NOT deal damage
            isRetreating = true;
        }
    }
}