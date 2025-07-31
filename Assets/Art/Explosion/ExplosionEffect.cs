using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private float lifeTime = 0.5f;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
