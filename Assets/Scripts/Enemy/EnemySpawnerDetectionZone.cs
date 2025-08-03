using UnityEngine;

public class EnemySpawnerDetectionZone : MonoBehaviour
{
    
    [SerializeField] public bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

}
