using UnityEngine;
using System.Collections;

public class ShieldDetectable : DetectableObject
{
    [Header("Shield Values")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldCooldown = 5f;

    private bool shieldActive;

    // Tracks the current remaining time
    private float cooldownTimer;

    private PlayerMovement player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        shieldActive = false;
        cooldownTimer = 0f;
    }

    public override void OnDetected()
    {
        if (!shieldActive)
        {
            StartCoroutine(ApplyShield());
        }
    }

    private IEnumerator ApplyShield()
    {
        if (shieldPrefab != null && player != null)
        {
            Instantiate(shieldPrefab, player.GetPlayerPosition(), Quaternion.identity);
            shieldActive = true;
            cooldownTimer = shieldCooldown;

            // Actively count down the time
            while (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            cooldownTimer = 0;
            shieldActive = false;
        }
    }

    // Public method for other scripts to check the status
    public bool IsShieldActive()
    {
        return shieldActive;
    }

    public float GetRemainingCooldown()
    {
        return cooldownTimer;
    }
}