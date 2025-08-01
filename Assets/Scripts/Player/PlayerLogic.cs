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

    [Header("Music References")]
    private bool isLowHealthMusicActive = false;

    [SerializeField] private CinemachineCamera virtualCamera;

    private void Awake()
    {
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
            return;
        }

        // Determine if health should be in the low state
        bool shouldBeLowHealth = (playerHealth <= 40);

        // Check if different from the current music state
        if (shouldBeLowHealth != isLowHealthMusicActive)
        {
            // If different, update FMOD and tracking variable
            GameMusicManager.Instance.SetLowHealthParameter(shouldBeLowHealth);
            isLowHealthMusicActive = shouldBeLowHealth;
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
