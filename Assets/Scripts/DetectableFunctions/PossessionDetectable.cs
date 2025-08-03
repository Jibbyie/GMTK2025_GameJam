using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class PossessionDetectable : DetectableObject
{
    [SerializeField] private float speed;
    public Rigidbody2D rb;
    private float defaultGrav = 0f;
    private float possessedGrav = 0f;

    private float horizontalInput;
    private float verticalInput;

    public bool isPossessed;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color possessedColour = Color.cyan;
    [SerializeField] private Color unpossessedColour = Color.red;

    [Header("Audio")]
    [SerializeField] private AudioClip possessionLoopSfx;
    private AudioSource audioSource;

    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform player;


    private Color originalColour;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Dynamic;
        defaultGrav = rb.gravityScale;
        speed = 5f;

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = possessionLoopSfx;

        virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                    //obj.rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = defaultGrav;
                }
            }

            // Possess yourself otherwise
            isPossessed = true;
            virtualCamera.Follow = rb.transform;
            StartCoroutine(FlashCoroutine(possessedColour));
            //rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = possessedGrav;

        }
        else
        {
            isPossessed = false;
            virtualCamera.Follow = player.transform;
            StartCoroutine(FlashCoroutine(unpossessedColour));
            //rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = defaultGrav;
        }
    }

    private void Update()
    {
        if(isPossessed)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Move();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
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
