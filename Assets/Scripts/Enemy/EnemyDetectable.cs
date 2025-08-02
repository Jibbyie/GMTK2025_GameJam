using UnityEngine;
using System.Collections;

public class EnemyDetectable : DetectableObject
{
    [Header("Health & Damage Values")]
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyMaxHealth = 100;
    [SerializeField] private int dealableDamage;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color flashColour = Color.red;

    [Header("Animation References")]
    [SerializeField] private Animator animator; 
    [SerializeField] private float deathAnimationDuration = 1f;
    private bool isDead = false;

    // Store original color so we can restore it
    private Color originalColour;

    [Header("Audio References")]
    [SerializeField] private AudioClip[] deathSfx;

    private Rigidbody2D enemyRB;
    private Collider2D enemyCollider;

    private void Awake()
    {
        enemyHealth = enemyMaxHealth;
        enemyRB = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>(); 
        }

        originalColour = spriteRenderer.color;
    }

    public override void OnDetected()
    {
        DealDamage(dealableDamage);
    }

    private void DealDamage(int damage)
    {
        StopAllCoroutines();

        StartCoroutine(FlashCoroutine());
        enemyHealth -= damage;

        if(enemyHealth <= 0 )
        {
            StartCoroutine(DeathSequence());
        }
    }
    // In EnemyDetectable.cs

    private IEnumerator DeathSequence()
    {
        // 1. Set state and freeze the enemy's movement
        isDead = true;
        enemyRB.bodyType = RigidbodyType2D.Static;

        // 2. Play death sound
        if (deathSfx != null && deathSfx.Length > 0)
        {
            int randomIndex = Random.Range(0, deathSfx.Length);
            AudioClip clipToPlay = deathSfx[randomIndex];
            // Play the sound while the object is still fully active in the scene
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
        }

        // 3. Now disable the collider and trigger the animation
        enemyCollider.enabled = false;
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // 4. Wait for the animation to finish
        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(this.gameObject);
    }

    private IEnumerator FlashCoroutine()
    {
        // set to flash colour
        spriteRenderer.color = flashColour;

        // Wait for flash duration
        yield return new WaitForSeconds(flashDuration);

        // Restore original colour
        spriteRenderer.color = originalColour;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
