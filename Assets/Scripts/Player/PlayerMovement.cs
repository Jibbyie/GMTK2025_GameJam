using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    private Rigidbody2D playerRB;
    private float horizontalInput;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Move();
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
}
