using UnityEngine;

public class ResetPositionDetectable : DetectableObject
{
    [Header("Target Object")]
    [Tooltip("The object that will be moved to this location.")]
    [SerializeField] private GameObject objectToMove;

    [Header("Audio")]
    [Tooltip("The sound to play when the object is moved.")]
    [SerializeField] private AudioClip moveSfx;

    private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public override void OnDetected()
    {
        // Ensure an object has been assigned in the Inspector before trying to move it.
        if (objectToMove == null)
        {
            Debug.LogWarning("No 'Object To Move' has been assigned to the MoveObjectOnDetect script.", this);
            return;
        }

        if (moveSfx != null)
        {
            audioSource.PlayOneShot(moveSfx);
        }

        // Move the target object to this object's current position
        objectToMove.transform.position = this.transform.position;
    }
}