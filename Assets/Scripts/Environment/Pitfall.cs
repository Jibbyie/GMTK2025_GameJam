using UnityEngine;

public class Pitfall : MonoBehaviour
{

    [SerializeField] public float Damage = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerLogic>().TakeDamage(Damage);
            collision.gameObject.GetComponent<PlayerLogic>().Respawn();
        }
    }

}
