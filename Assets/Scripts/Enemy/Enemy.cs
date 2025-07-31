using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Values & State")]
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private bool isRetreating;
    [SerializeField] private float retreatTimer;
    [SerializeField] private float retreatDuration = 1f;

    private Rigidbody2D enemyRB;
    private Vector3 retreatDirection;

    private PlayerMovement player;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        retreatDuration = 1f;
        retreatTimer = retreatDuration;

        player = FindFirstObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if(isRetreating)
        {
            var enemyPosition = enemyRB.transform.position;
            var playerPosition = player.GetPlayerPosition();
            var distanceBetween = enemyPosition - playerPosition;
            retreatDirection = distanceBetween.normalized;

            enemyRB.transform.position += retreatDirection * enemySpeed * Time.deltaTime;
            retreatTimer -= Time.deltaTime;

            if(retreatTimer <= 0 )
            {
                isRetreating = false;
                retreatTimer = retreatDuration;
            }
        }
        if(!isRetreating)
        {
            enemyRB.transform.position = Vector2.MoveTowards(transform.position, player.GetPlayerPosition(), enemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
        {
            isRetreating = true;
            retreatTimer = retreatDuration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
        {
            isRetreating = true;
            retreatTimer = retreatDuration;
        }
    }
}
