using UnityEngine;

public class GroundDetector : MonoBehaviour
{

    [SerializeField] private SpriteRenderer SR;
    private float RayDistance;

    private void Start()
    {
        RayDistance = SR.bounds.extents.y + 0.1f;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * RayDistance, Color.red);
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, RayDistance, LayerMask.GetMask("Ground"));
    }

}
