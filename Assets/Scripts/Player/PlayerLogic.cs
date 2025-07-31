using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float playerHealth;

    private void Awake()
    {
        maxHealth = 100f;
        playerHealth = maxHealth;
    }

    private void Update()
    {
        if(playerHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }
}
