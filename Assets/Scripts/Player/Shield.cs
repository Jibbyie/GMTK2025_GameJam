using UnityEngine;

public class Shield : MonoBehaviour
{
    private PlayerMovement player;
    Rigidbody2D shieldRb;
    [SerializeField] private float lifeTime = 1f;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        shieldRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(player != null)
        {
            shieldRb.transform.position = player.GetPlayerPosition();
        }

        Vector3 shieldScale = transform.localScale;

        if (player.IsFacingRight())
        {
            shieldScale.x = Mathf.Abs(shieldScale.x);
        }
        else
        {
            shieldScale.x = -Mathf.Abs(shieldScale.x);
        }

        // Apply the updated scale to the shield
        transform.localScale = shieldScale;

        Destroy(this.gameObject, lifeTime);
    }

}
