using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PossessionDetectable : DetectableObject
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private float horizontalInput;
    private float verticalInput;

    private PlayerMovement playerMovement;
    public bool isPossessed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        speed = 5f;
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
                    obj.rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }

            // Possess yourself otherwise
            isPossessed = true;
            rb.bodyType = RigidbodyType2D.Kinematic;

        }
        else
        {
            isPossessed = false;
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

    public bool GetPossessionState()
    {
        return isPossessed;
    }
}
