using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    [Header("Settings")]
    [SerializeField] private float doorSpeed;

    [Header("Audio")] 
    [SerializeField] private AudioClip doorMovingSfx;

    private Transform currentTarget;
    private AudioSource audioSource; 

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (currentTarget == topPoint) return;
        currentTarget = topPoint;
        PlayMovingSound(); 
    }

    public void CloseDoor()
    {
        if (currentTarget == bottomPoint) return;
        currentTarget = bottomPoint;
        PlayMovingSound();
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Check if the door is already at the target destination.
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            // We've arrived, so clear the target and stop moving.
            currentTarget = null;

            // Stop the sound since we've arrived. 
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return;
        }

        // If we have a target and we are not there yet, move towards it.
        Vector2 newPosition = Vector2.MoveTowards(transform.position, currentTarget.position, doorSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void PlayMovingSound()
    {

        if (!audioSource.isPlaying)
        {
            audioSource.clip = doorMovingSfx;
            audioSource.Play();
        }
    }
}