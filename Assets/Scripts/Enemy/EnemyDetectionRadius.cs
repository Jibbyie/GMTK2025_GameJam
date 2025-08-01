using UnityEngine;

public class EnemyDetectionRadius : MonoBehaviour
{
    public bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only set detected to true if the player enters
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only set detected to false if the player leaves
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}