using UnityEngine;

public class TeleportationDetectable : DetectableObject
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnDetected()
    {
        player.transform.position = this.transform.position;
    }
}
