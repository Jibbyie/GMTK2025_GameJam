using UnityEngine;
using System.Collections;

public class TangibleDetectable : DetectableObject
{
    [Header("Values")]
    [SerializeField] private float activeDuration = 5f;
    [SerializeField] private float activeTimer;

    [Header("References")]
    [SerializeField] private bool isTangible;
    [SerializeField] private Collider2D internalCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        activeTimer = activeDuration;

        isTangible = false;
        internalCollider = GetComponent<Collider2D>();
        internalCollider.enabled = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set alpha to 25% transparent
        Color color = spriteRenderer.color;
        color.a = 0.25f;
        spriteRenderer.color = color;
    }
    public override void OnDetected()
    {
        if (isTangible) return;

        if(!isTangible)
        {
            isTangible = true;
            internalCollider.enabled = true;

            // Stop any previous fades and start fading in
            StopAllCoroutines();
            StartCoroutine(FadeAlpha(1f, 0.5f));

            activeTimer = activeDuration;
        }
    }

    private void Update()
    {
        if (!isTangible) return;

        if (isTangible)
        {
            activeTimer -= Time.deltaTime;
            if(activeTimer <= 0)
            {
                isTangible = false;
                internalCollider.enabled = false;

                // Stop any previous fades and start fading out
                StopAllCoroutines();
                StartCoroutine(FadeAlpha(0.25f, 0.5f));
            }
        }
    }

    private IEnumerator FadeAlpha(float targetAlpha, float duration)
    {
        Color currentColor = spriteRenderer.color;
        float startAlpha = currentColor.a;
        float elapsedTimer = 0f;

        while (elapsedTimer < duration)
        {
            // Add time since last frame to elapsed time
            elapsedTimer += Time.deltaTime;

            // Calculate progress from 0 to 1
            float progress = Mathf.Clamp01(elapsedTimer / duration);

            // Lerp alpha and update colour
            currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            spriteRenderer.color = currentColor;

            // Wait for next frame
            yield return null;

            // Ensure final alpha is set
            currentColor.a = targetAlpha;
            spriteRenderer.color = currentColor;
        }
    }
}
