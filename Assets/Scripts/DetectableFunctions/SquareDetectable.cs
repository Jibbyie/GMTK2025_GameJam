using UnityEngine;

public class SquareDetectable : DetectableObject
{
    public override void OnDetected()
    {
        Debug.Log("I am a square!");
    }
}
