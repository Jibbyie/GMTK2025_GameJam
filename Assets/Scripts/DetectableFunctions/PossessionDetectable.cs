using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PossessionDetectable : DetectableObject
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private float horizontalInput;
    private float verticalInput;

    public bool isPossessed;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color possessedColour = Color.cyan;
    [SerializeField] private Color unpossessedColour = Color.red;

    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform player;


    private Color originalColour;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        speed = 5f;

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        originalColour = spriteRenderer.color;
    }

    public override void OnDetected()
    {
        if(!isPossessed)
        {
            PossessionDetectable[] allPossessables = FindObjectsByType<PossessionDetectable>(FindObjectsSortMode.None);
            foreach (PossessionDetectable obj in allPossessables)
            {
                if(obj != this) // don't possess yourself
                {
                    obj.isPossessed = false;
                    virtualCamera.Follow = player.transform;
                    StartCoroutine(FlashCoroutine(unpossessedColour));
                    obj.rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }

            // Possess yourself otherwise
            isPossessed = true;
            virtualCamera.Follow = rb.transform;
            StartCoroutine(FlashCoroutine(possessedColour));
            rb.bodyType = RigidbodyType2D.Kinematic;

        }
        else
        {
            isPossessed = false;
            virtualCamera.Follow = player.transform;
            StartCoroutine(FlashCoroutine(unpossessedColour));
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void Update()
    {
        if(isPossessed)
        {
            Move();
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
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

    public bool GetPossessionState()
    {
        return isPossessed;
    }
}
