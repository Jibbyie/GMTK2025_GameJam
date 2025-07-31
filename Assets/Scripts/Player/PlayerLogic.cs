using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float playerHealth;

    private PossessionDetectable possession;

    private void Awake()
    {
        maxHealth = 100f;
        playerHealth = maxHealth;
    }

    private void Update()
    {
        if(playerHealth <= 0)
        {
            playerHealth = 0;
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;

        PossessionDetectable[] allPossessables = FindObjectsByType<PossessionDetectable>(FindObjectsSortMode.None);
        foreach(PossessionDetectable obj in allPossessables)
        {
            obj.isPossessed = false;
        }
    }

    public float GetHealth()
    {
        return playerHealth;
    }
}
