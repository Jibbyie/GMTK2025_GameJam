using UnityEngine;

public class EnemyDetectionRadius : MonoBehaviour
{

    [SerializeField] private CircleCollider2D detectionRadius;
    public bool playerDetected = false;

    private void Awake()
    {
        detectionRadius = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerDetected = collision.gameObject.CompareTag("Player");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

}
