using UnityEngine;

public class PlayerDetectable : DetectableObject
{
    public override void OnDetected()
    {
        Debug.Log("I am the player!");
    }
}
