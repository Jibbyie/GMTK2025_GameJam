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
        Destroy(this.gameObject, lifeTime);
    }

}
