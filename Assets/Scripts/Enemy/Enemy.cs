using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Wandering,
        Chasing,
        Retreating
    }

    [Header("Enemy Values")]
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float damageToPlayer = 20f;
    bool isFacingRight = false;

    [Header("State Timers")]
    [SerializeField] private float retreatDuration = 1f;
    [SerializeField] private float wanderDuration = 3f;
    private float retreatTimer;
    private float wanderTimer;

    [Header("Component References")]
    [SerializeField] private EnemyDetectionRadius ERD;
    [SerializeField] private EnemyDetectable ED;
    private Rigidbody2D enemyRB;
    private PlayerMovement player;
    private PlayerLogic playerLogic;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shieldHit;
    [SerializeField] private AudioClip detectionSfx;

    // Private variables for state and movement
    private State currentState;
    private float wanderDirection = 1f;
    private Vector2 retreatDirection;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        ED = GetComponent<EnemyDetectable>();
        player = FindFirstObjectByType<PlayerMovement>();
        playerLogic = FindFirstObjectByType<PlayerLogic>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Start in the Wandering state.
        currentState = State.Wandering;
        wanderTimer = wanderDuration;
    }

    // Update is for logic, timers, and state transitions.
    private void Update()
    {
        FlipSprite();

        switch (currentState)
        {
            case State.Wandering:
                HandleWanderingState();
                break;
            case State.Chasing:
                HandleChasingState();
                break;
            case State.Retreating:
                HandleRetreatingState();
                break;
        }
    }

    // FixedUpdate is only for applying physics-based movement.
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Wandering:
                ApplyWanderMovement();
                break;
            case State.Chasing:
                ApplyChaseMovement();
                break;
            case State.Retreating:
                ApplyRetreatMovement();
                break;
        }
    }

    private void FlipSprite()
    {
        if (isFacingRight && enemyRB.linearVelocity.x < 0f || !isFacingRight && enemyRB.linearVelocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // State Logic (The "Brain")

    private void HandleWanderingState()
    {
        // Transition to Chasing if player is detected.
        if (ERD != null && ERD.playerDetected)
        {
            if (detectionSfx != null)
            {
                AudioSource.PlayClipAtPoint(detectionSfx, transform.position);
            }

            currentState = State.Chasing;
            return;
        }

        // Handle wander timer.
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            wanderDirection *= -1; // Flip direction
            wanderTimer = wanderDuration;
        }
    }

    private void HandleChasingState()
    {
        // Transition to Wandering if player is no longer detected.
        if (ERD != null && !ERD.playerDetected)
        {
            currentState = State.Wandering;
            wanderTimer = wanderDuration; // Reset wander timer

            // Reset the enemy's velocity completely to prevent carrying over
            // vertical momentum from the chase or retreat states.
            enemyRB.linearVelocity = Vector2.zero;
        }
    }

    private void HandleRetreatingState()
    {
        // Count down the retreat timer.
        retreatTimer -= Time.deltaTime;
        if (retreatTimer <= 0)
        {
            // When retreat is over, go back to chasing (player is likely still nearby).
            currentState = State.Chasing;
        }
    }

    // Movement Logic (The "Body")

    private void ApplyWanderMovement()
    {
        enemyRB.linearVelocity = new Vector2(wanderDirection * wanderSpeed, enemyRB.linearVelocity.y);
    }

    private void ApplyChaseMovement()
    {
        if (player == null) return;
        Vector2 directionToPlayer = (player.GetPlayerPosition() - transform.position).normalized;
        enemyRB.linearVelocity = directionToPlayer * enemySpeed;
    }

    private void ApplyRetreatMovement()
    {
        enemyRB.linearVelocity = retreatDirection * enemySpeed;
    }


    // Collision and Public Methods

    // Public method for the PlayerLogic fail-safe to call.
    public void ActivateRetreat()
    {
        if (currentState == State.Retreating) return; // Don't restart if already retreating.

        currentState = State.Retreating;
        retreatTimer = retreatDuration;

        // Calculate retreat direction once upon activation.
        if (player != null)
        {
            retreatDirection = (transform.position - player.GetPlayerPosition()).normalized;
        }
    }

    // Handles physical collision with the player.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ED.IsDead()) return;
            playerLogic.TakeDamage(damageToPlayer, this);
            ActivateRetreat();
        }
    }

    // Handles trigger collision with the shield.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            if (ED.IsDead()) return;
            audioSource.PlayOneShot(shieldHit);
            ActivateRetreat();
        }
    }
}
