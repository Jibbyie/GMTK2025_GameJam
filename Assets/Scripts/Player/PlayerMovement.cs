using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float walkSpeed = 5f;

    [Header("Jumping Values")]
    [SerializeField] private float initialJumpForce = 1f;
    [Tooltip("Max duration to hold jump")][SerializeField] private float maxJumpHoldTime = 0.25f;
    [Tooltip("Added force while holding jump")][SerializeField] private float additionalJumpForce = 30f;
    [SerializeField] private float jumpHoldTimer;
    [SerializeField] private bool isVariableJumping;
    [SerializeField] private bool isGrounded = false;

    private Rigidbody2D playerRB;
    Animator animator;
    private float horizontalInput;
    bool isFacingRight = true;
    private LayerMask GroundLayer;
    private Collider2D GroundDetector;

    private PossessionDetectable possession;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        possession = FindFirstObjectByType<PossessionDetectable>();
        GroundDetector = GetComponentInChildren<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();

        GroundLayer = LayerMask.NameToLayer("Ground");
        gameObject.layer = GroundLayer;
    }
    private void Update()
    {
        animator.SetBool("isJumping", !isGrounded);
        FlipSprite();

        // Only allow jumping if NOTHING is currently possessed.
        if (!IsAnythingPossessed())
        {
            HandleJumpInput();
        }
        else
        {
            playerRB.linearVelocity = new Vector2(0, playerRB.linearVelocity.y);
        }
    }

    private void FixedUpdate()
    {
        // Only allow movement if NOTHING is currently possessed.
        if (!IsAnythingPossessed())
        {
            Move();
            animator.SetFloat("yVelocity", playerRB.linearVelocity.y);
        }
    }

    public void SetWalkSpeed(float newSpeed)
    {
        walkSpeed = newSpeed;
    }

    public void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        playerRB.linearVelocity = new Vector2(horizontalInput * walkSpeed, playerRB.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(playerRB.linearVelocity.x));
    }

    private void HandleJumpInput()
    {
        // Check for jump button press (start of jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StartJump();
        }

        // Check for jump button held
        if (Input.GetButton("Jump") && isVariableJumping)
        {
            UpdateVariableJump();
        }

        // Check for jump button release or max jump height
        if (Input.GetButtonUp("Jump") || ShouldEndVariableJump())
        {
            EndJump();
        }

    }

    private void StartJump()
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, initialJumpForce);

        isGrounded = false;
        isVariableJumping = true;
        jumpHoldTimer = 0;
    }

    private void UpdateVariableJump()
    {
        jumpHoldTimer += Time.deltaTime;
        if (playerRB.linearVelocity.y > 0)
        {
            if (jumpHoldTimer < maxJumpHoldTime)
            {
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, playerRB.linearVelocity.y + (additionalJumpForce * Time.deltaTime));
            }
        }
        else
        {
            EndJump();
        }
    }

    private void EndJump()
    {
        isVariableJumping = false;
        jumpHoldTimer = 0;
    }
    private bool ShouldEndVariableJump()
    {
        if (jumpHoldTimer >= maxJumpHoldTime)
        {
            return true;
        }
        if (playerRB.linearVelocity.y <= 0)
        {
            return true;
        }
        if (isGrounded)
        {
            return true;
        }
        if (!isVariableJumping)
        {
            return true;
        }
        return false;
    }

    private void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private bool IsAnythingPossessed()
    {
        // Find all possessable objects.
        PossessionDetectable[] allPossessables = FindObjectsByType<PossessionDetectable>(FindObjectsSortMode.None);

        foreach (PossessionDetectable obj in allPossessables)
        {
            // If we find ANY object that is possessed...
            if (obj.GetPossessionState())
            {
                return true;
            }
        }

        // If the loop finishes without finding any possessed objects, return false.
        return false;
    }

    public Vector3 GetPlayerPosition()
    {
        if (playerRB == null)
        {
            return Vector3.zero;
        }
        return playerRB.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            isGrounded = false;

        }
    }
}