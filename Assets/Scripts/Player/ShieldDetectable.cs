using UnityEngine;
using System.Collections;

public class ShieldDetectable : DetectableObject
{
    [Header("Shield Values")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldCooldown = 5f;

    bool shieldActive;
    
    private PlayerMovement player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        shieldActive = false;
    }

    public override void OnDetected()
    {
        if(!shieldActive)
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

            Debug.Log(shieldActive);

            yield return new WaitForSeconds(shieldCooldown);
            shieldActive = false;

            Debug.Log(shieldActive);
        }
    }
}