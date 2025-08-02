using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Audio")]
    [SerializeField] private AudioClip[] hitSfxs;
    [SerializeField] private AudioClip deathSfx;
    private AudioSource audioSource;

    [SerializeField] private CinemachineCamera virtualCamera;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float deathAnimationDuration = 2f; // Placeholder for animation length
    private bool isDead = false;

    private void Awake()
    {
        playerHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        audioSource = GetComponent<AudioSource>();

        virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        originalColour = spriteRenderer.color;
    }

    private void Update()
    {

        // Check for death and make sure the death sequence hasn't started
        if (playerHealth <= 0 && !isDead)
        {
            StartCoroutine(DeathSequence());
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
        // If the player is already in the death sequence, do nothing.
        if (isDead) return;

        if (hitSfxs != null && hitSfxs.Length > 0)
        {
            // 1. Pick a random index from the array
            int randomIndex = Random.Range(0, hitSfxs.Length);
            AudioClip randomHitClip = hitSfxs[randomIndex];

            // 2. Play the randomly selected clip
            AudioSource.PlayClipAtPoint(randomHitClip, Camera.main.transform.position);
        }

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

    private IEnumerator DeathSequence()
    {
        // --- 1. Start of Death ---
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        Rigidbody2D playerRB = GetComponent<Rigidbody2D>();
        playerRB.bodyType = RigidbodyType2D.Static;

        // --- 2. Play Sound & Trigger Animation ---
        if (deathSfx != null)
        {
            audioSource.PlayOneShot(deathSfx, 0.5f);
        }

        // This triggers the death animation. 
        animator.SetTrigger("Death");


        // --- 3. Wait for Animation to Finish ---
        yield return new WaitForSeconds(deathAnimationDuration);


        // --- 4. Reset Music and Reload Scene ---
        // Explicitly set the music back to the default state before reloading.
        GameMusicManager.Instance.SetLowHealthParameter(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
