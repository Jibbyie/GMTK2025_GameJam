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
    [SerializeField] private GameObject explosionEffect;

    // Store original color so we can restore it
    private Color originalColour;

    private Rigidbody2D enemyRB;

    private void Awake()
    {
        enemyHealth = enemyMaxHealth;
        enemyRB = GetComponent<Rigidbody2D>();

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
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
}
