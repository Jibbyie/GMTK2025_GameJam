using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    [SerializeField] private float doorSpeed;

    private Transform currentTarget;

    public void OpenDoor()
    {
        if (currentTarget == topPoint) return; // already opening/open
        currentTarget = topPoint;
    }

    public void CloseDoor()
    {
        if (currentTarget == bottomPoint) return; // already closing/closed
        currentTarget = bottomPoint;
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Check if the door is already at the target destination.
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            // We've arrived, so clear the target and stop moving.
            currentTarget = null;
            return;
        }

        // If we have a target and we are not there yet, move towards it.
        Vector2 newPosition = Vector2.MoveTowards(transform.position, currentTarget.position, doorSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
