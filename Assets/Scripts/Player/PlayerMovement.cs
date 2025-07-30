using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    private Rigidbody2D playerRB;
    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        playerRB.linearVelocity = new Vector2(horizontalInput * walkSpeed, playerRB.linearVelocity.y);
    }
}
