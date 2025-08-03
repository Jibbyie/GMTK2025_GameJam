using UnityEngine;

public class PlatformDetectable : DetectableObject
{
    [SerializeField] private float platformSpeed = 5f;
    private Rigidbody2D platformRB;
    private bool platformMoved;
    private bool isMoving;
    private Transform currentTarget;

    [Header("Points")]
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    private void Awake()
    {
        platformRB = GetComponent<Rigidbody2D>();
    }

    public override void OnDetected()
    {
        if (platformRB != null)
        {

            if(!platformMoved)
            {
                currentTarget = topPoint;
                platformMoved = true;
            }
            else
            {
                currentTarget = bottomPoint;
                platformMoved = false;
            }

            isMoving = true;
            Debug.Log("Platform moving to: " + currentTarget);
        }
    }

    private void Update()
    {
        if(isMoving && currentTarget  != null)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        // Move towards the target
        Vector2 newPosition = Vector2.MoveTowards(transform.position, currentTarget.position, platformSpeed * Time.deltaTime);
        transform.position = newPosition;

        // Check if we've reached the target
        if(Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            isMoving = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
