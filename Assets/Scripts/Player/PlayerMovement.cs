using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float JumpSpeed = 5f;
    private Rigidbody2D playerRB;
    private float horizontalInput;
    private GroundDetector GD;

    //Coyote time and jump buffering variables
    [SerializeField] private float CoyoteTime = 0.1f;
    public float CoyoteTimer;
    [SerializeField] private float JumpBuffer = 0.1f;
    public float JumpBufferTimer;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GD = GetComponent<GroundDetector>();
    }
    private void Update()
    {
        Move();
        Jump();
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
}
