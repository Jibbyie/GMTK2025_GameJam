using UnityEngine;

public class CircleDetectable : DetectableObject
{
    public override void OnDetected()
    {
        Debug.Log("I am a circle!");
    }
}
