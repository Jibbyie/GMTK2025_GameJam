using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float JumpSpeed = 5f;
    private Rigidbody2D playerRB;
    private float horizontalInput;
    private GroundDetector GD;
    private PossessionDetectable possession;

    //Coyote time and jump buffering variables
    [SerializeField] private float CoyoteTime = 0.1f;
    public float CoyoteTimer;
    [SerializeField] private float JumpBuffer = 0.1f;
    public float JumpBufferTimer;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GD = GetComponent<GroundDetector>();
        possession = FindFirstObjectByType<PossessionDetectable>();
    }
    private void Update()
    {
        // Only allow jumping if NOTHING is currently possessed.
        if (!IsAnythingPossessed())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Only allow movement if NOTHING is currently possessed.
        if (!IsAnythingPossessed())
        {
            Move();
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
    }

    public void Jump()
    {
        if (GD.IsGrounded())
        {
            CoyoteTimer = CoyoteTime;
        }
        else
        {
            CoyoteTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpBufferTimer = JumpBuffer;
        }
        else
        {
            JumpBufferTimer -= Time.deltaTime;
        }


        if (JumpBufferTimer > 0f && CoyoteTimer > 0f)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, JumpSpeed);
            CoyoteTimer = 0f;
            JumpBufferTimer = 0f;
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

}
