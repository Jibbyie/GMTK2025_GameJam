using UnityEngine;

public class DetectableObject : MonoBehaviour
{
    private string detectionMessage = "Default detection message";

    public virtual void OnDetected()
    {
        // Default behaviour
        Debug.Log(detectionMessage);
    }
}
