using UnityEngine;

public class Pitfall : MonoBehaviour
{

    [SerializeField] private Transform respawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = respawnPoint.position;
        }
    }

}
