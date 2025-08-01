using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float playerHealth;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color hit = Color.red;
    private Color originalColour;

    [SerializeField] private CinemachineCamera virtualCamera;

    private PossessionDetectable possession;

    private void Awake()
    {
        maxHealth = 100f;
        playerHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        originalColour = spriteRenderer.color;
    }

    private void Update()
    {
        if(playerHealth <= 0)
        {
            playerHealth = 0;
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage, Enemy enemy)
    {
        playerHealth -= damage;

        if (virtualCamera.Follow != transform)
        {
            virtualCamera.Follow = transform;
        }

        StartCoroutine(FlashCoroutine(hit));
        PossessionDetectable[] allPossessables = FindObjectsByType<PossessionDetectable>(FindObjectsSortMode.None);
        foreach(PossessionDetectable obj in allPossessables)
        {
            obj.isPossessed = false;
            obj.rb.bodyType = RigidbodyType2D.Dynamic;
        }

    }

    public float GetHealth()
    {
        return playerHealth;
    }

    private IEnumerator FlashCoroutine(Color colorToBe)
    {
        // set to flash colour
        spriteRenderer.color = colorToBe;

        // Wait for flash duration
        yield return new WaitForSeconds(flashDuration);

        // Restore original colour
        spriteRenderer.color = originalColour;
    }
}
